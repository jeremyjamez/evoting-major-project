import { Button, Grid, Input, Select, Spacer, Tabs, Row, Col, Avatar, Card } from "@geist-ui/react"
import { useEffect, useState } from "react"
import ConstituencyMap from "../../components/ConstituencyMap"
import { useConstituencies, useConstituency } from "../../utils/swr-utils"
import DashboardLayout from "./layout"
import https from "https"
import { useForm } from "react-hook-form"
import { Minus, Plus } from "@geist-ui/react-icons"
import jwt from 'jsonwebtoken'
import { parseCookies } from 'nookies'
import moment from 'moment'

const Constituencies = ({ token }) => {

    const [mapWidth, setMapWidth] = useState(24)
    const [showCard, setShowCard] = useState(0)
    const [constituencyName, setConstituencyName] = useState()
    const [constit, setConstituency] = useState()
    const [candid, setCandidate] = useState()
    const [votersList, setVoters] = useState()

    const handleClick = (name) => {
        setConstituencyName(name)
    }

    const { constituency, candidate, voters } = useConstituency(constituencyName, token)

    useEffect(() => {
        setConstituency(constituency)
        setCandidate(candidate)
        setVoters(voters)
    },[constituencyName, constituency, candidate, voters])
    

    return (
        <DashboardLayout>
            <div className="map-container">
                <ConstituencyMap onClick={handleClick} />
                {/* <div id="mapControls">
                    <Grid.Container gap={2}>
                        <Grid>
                            <Button type="secondary" icon={<Minus />} auto />
                        </Grid>
                        <Grid>
                            <Button type="secondary" icon={<Plus />} auto />
                        </Grid>
                    </Grid.Container>
                </div> */}
            </div>
            <div className="candidate-container">
                <Card shadow style={{ height: '100%' }}>
                    <Row style={{ textAlign: 'center' }}>
                        <Col>
                            {
                                !candid ? '' :
                                    <>
                                        <Avatar size="large" src={candid.photo} />
                                        <h3>{candid.lastName + ', ' + candid.firstName}</h3>
                                    </>
                            }
                            <h4>{!constit ? '' : constit.name}</h4>

                            {
                                !votersList ? '' :
                                    <h5>{votersList.length} Registered Voters</h5>
                            }
                        </Col>
                    </Row>
                </Card>
            </div>
            <style jsx>{`
                .candidate-container {
                    position: absolute;
                    right: 16px;
                    top: 10%;
                    bottom: 5%;
                    width: 20%;
                }
                        .map-container {
                            height: 100%;
                        }
                        #mapControls {
                            position: absolute;
                            bottom: 24px;
                            left: 5%;
                        }
                    `}</style>
        </DashboardLayout>
    )
}

export async function getServerSideProps(context) {
    const cookies = parseCookies(context)

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

export default Constituencies;