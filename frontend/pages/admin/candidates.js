import React, { useEffect, useState, useMemo } from "react";
import {Tabs} from '@geist-ui/react'
import DashboardLayout from "./layout";
import moment from "moment";
import DataTable from "react-data-table-component";
import FilterComponent from "../../components/FilterComponent";
import { useForm } from "react-hook-form";
import { useCandidates, useConstituencies, useElections } from "../../utils/swr-utils";
import jwt from 'jsonwebtoken'
import { parseCookies } from 'nookies'

export default function Candidates({ token }) {
    const { candidates } = useCandidates(token)
    const { constituencies } = useConstituencies(token)
    const { elections } = useElections(token)

    const candidateCol = useMemo(
        () => [
            {
                name: 'Candidate ID',
                selector: 'candidateId',
            },
            {
                name: 'Constituency ID',
                selector: 'constituencyId',
            },
            {
                name: 'Full Name',
                selector: (val) => {
                    return val.lastName + ", " + val.firstName
                }
            },
            {
                name: 'Affiliation',
                selector: 'affiliation'
            }
        ], []);

    return (
        <DashboardLayout>
            <Tabs initialValue="1" style={{margin: '16px', width: '100%'}}>
                <Tabs.Item label="all candidates" value="1">
                    <DataTable
                        columns={candidateCol}
                        data={candidates}
                        highlightOnHover
                        noHeader
                        pagination />
                </Tabs.Item>
            </Tabs>
        </DashboardLayout>
    )
}

export async function getServerSideProps(context) {
    const cookies = parseCookies(context)

    const token = cookies.to
    const decodedToken = jwt.decode(token, { complete: true })
    var dateNow = moment(moment().valueOf()).unix()

    if(token == null){
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

/* export async function getStaticProps() {
    const httpsAgent = new https.Agent({
        rejectUnauthorized: false,
    });

    const [constituencyData, electionData] = await Promise.all([
        fetch(`https://localhost:44387/api/Constituencies`, { agent: httpsAgent }).then(r => r.json()),
        fetch(`https://localhost:44387/api/Elections`, { agent: httpsAgent }).then(r => r.json())
    ]);

    return {
        props: {
            constituencyData,
            electionData
        }
    }
} */