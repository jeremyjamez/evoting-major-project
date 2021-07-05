import { Grid, Card, Text, Select, Button, Spacer, Modal, useModal, useCurrentState } from "@geist-ui/react";
import { BarChart2 } from "@geist-ui/react-icons";
import DashboardLayout from "./layout";
import { Bar, HorizontalBar } from "@reactchartjs/react-chart.js";
import moment from "moment";
import nookies from 'nookies'
import jwt from 'jsonwebtoken'
import { useCandidates, useConstituencies, useElections, useGetElection, useGetParty, usePoliticalParties } from "../../utils/swr-utils";
import DataTable from 'react-data-table-component'
import { useEffect, useState } from "react";
import https from 'https'

const options = {
    scales: {
        yAxes: [
            {
                ticks: {
                    beginAtZero: true,
                },
            },
        ],
    },
}

export default function Home({ token }) {

    const { elections } = useElections(token)
    const { constituencies } = useConstituencies(token)
    const { candidates } = useCandidates(token)
    //const { election } = useGetElection(token, Math.trunc(moment().valueOf() / 1000).toString())

    const { setVisible, bindings } = useModal(false)

    const voterPopulationData = {
        labels: ['2020', '2019', '2018', '2017'],
        datasets: [
            {
                label: 'Population of Voters',
                backgroundColor: 'rgba(243, 85, 142, 1)',
                data: [1932083, 1897146, 1934860, 1905278]
            },
            {
                label: 'General Population',
                backgroundColor: 'rgba(156, 29, 231, 1)',
                data: [2812032, 2734092, 2730982, 2725882]
            }
        ]
    }

    const columns = [
        {
            name: 'Candidate ID',
            selector: 'candidateId'
        },
        {
            name: 'Constituency ID',
            selector: 'constituencyId'
        },
        {
            name: 'Election ID',
            selector: 'electionId'
        },
        {
            name: 'Total Votes',
            selector: 'totalVotes'
        }
    ]

    const [votes, setVotes, votesRef] = useCurrentState()
    const [voteData, setVoteData, voteDataRef] = useCurrentState()
    const { parties } = usePoliticalParties(token)

    const generateVoteGraph = (data) => {
        if (data) {
            let conArr = constituencies.filter((con) =>
                data.some((entry) => entry.constituencyId === con.constituencyId))
                .map((con) => con.name)

            let canArr = data.filter((d) =>
                candidates.some((entry) => entry.candidateId === d.candidateId))
                .map((entry, index) => {
                    const candidate = candidates.find(
                        (can) => can.candidateId === entry.candidateId
                    )

                    if (candidate === null) {
                        return null
                    }

                    const constituency = constituencies.find((con) => con.constituencyId === entry.constituencyId)
                    const conIdx = conArr.indexOf(constituency.name)
                    const party = parties.find((par) => par.partyId === candidate.affiliation)
                    return {
                        label: candidate.lastName + ", " + candidate.firstName,
                        backgroundColor: party.colour,
                        data: new Array(conIdx).fill(0).concat([entry.totalVotes])
                    }
                })

            const votesData = {
                labels: conArr,
                datasets: canArr
            }

            return votesData
        }
        return null
    }

    const handleElectionChange = (e) => {
        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });
        fetch(`${process.env.NEXT_PUBLIC_API_URL}/votes/countvotes/${e}`, {
            agent: httpsAgent,
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            }
        })
            .then(res => {
                if (res.ok) {
                    return res.json()
                }
            })
            .then(data => {
                setVotes(data)
                setVoteData(generateVoteGraph(data))
            })
    }

    return (
        <DashboardLayout>
            <div style={{ margin: '16px', width: '100%' }}>
                <Grid.Container gap={2} >
                    <Grid xs={24} xl={24}>
                        {/* <Card type="dark" hoverable>
                            election && <h3>{election.electionType} {moment(election.electionDate).format("DD/MM/YYYY hh:mm a")}</h3>
                        </Card> */}
                    </Grid>
                    <Grid xs={24} xl={7}>
                        <Card shadow>
                            <h3>Voter Population</h3>
                            <Bar data={voterPopulationData} />
                        </Card>
                    </Grid>
                    <Grid xs={24} xl={17}>
                        <Card shadow>
                            <Grid.Container justify="space-between">
                                <Grid>
                                    <h3>Votes <Text small>(per election)</Text></h3>
                                </Grid>
                                <Grid xs={8}>
                                    <Button iconRight={<BarChart2 size={36} />} type="secondary" onClick={() => setVisible(true)} auto size="large"></Button>
                                    <Spacer x={2} />
                                    <Select size="large" width="100%" onChange={handleElectionChange}>
                                        {
                                            elections && elections.map((elec) => {
                                                return <Select.Option key={elec.id} value={elec.id}>
                                                    <Text size="1.25rem" span>{moment(elec.date * 1000).year() + " " + elec.type + " - " + moment(elec.date * 1000).format('DD/MM')}</Text>
                                                </Select.Option>
                                            })
                                        }
                                    </Select>
                                </Grid>
                                <Grid xs={24} style={{ display: 'block' }}>
                                    <DataTable
                                        columns={columns}
                                        data={votes}
                                        pagination
                                        highlightOnHover
                                        noHeader
                                    />
                                </Grid>
                            </Grid.Container>
                        </Card>
                    </Grid>
                </Grid.Container>
            </div>
            <Modal {...bindings} width="100%">
                <Modal.Content>
                    <Bar data={voteDataRef.current} options={options}/>
                </Modal.Content>
                <Modal.Action passive onClick={() => setVisible(false)}>Close</Modal.Action>
            </Modal>
            <style global jsx>{`
                .rdt_TableCol {
                    font-size: 1.25rem;
                }

                .rdt_TableCell {
                    font-size: 1.2rem;
                }
            `}</style>
        </DashboardLayout>
    )
}

export async function getServerSideProps(ctx) {
    const cookies = nookies.get(ctx)

    const token = cookies.to
    const decodedToken = jwt.decode(token, { complete: true })
    var dateNow = moment(moment().valueOf()).unix()

    if (token == null) {
        return {
            redirect: {
                destination: '/admin/login',
                permanent: false
            }
        }
    }

    if (decodedToken !== null && decodedToken.payload.exp < dateNow) {
        return {
            redirect: {
                destination: '/admin/login',
                permanent: false
            }
        }
    }

    return {
        props: {
            token
        }
    }
}