import { Button, Grid, Input, Select, Spacer, Tabs, Row, Col, Avatar, Card } from "@geist-ui/react"
import { useEffect, useState } from "react"
import ConstituencyMap from "../../components/ConstituencyMap"
import { useConstituencies, useConstituencyMP } from "../../utils/swr-utils"
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

    const handleClick = (name) => {
        setConstituencyName(name)
    }

    const { constituency, candidate, mp, member, voters } = useConstituencyMP(constituencyName, token)
    const { register, setValue, handleSubmit } = useForm()

    const handleParishChange = e => {
        setValue("parish", e)
    }

    useEffect(() => {
        register("parish")
    }, [register])

    const onSubmit = (data) => {
        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });

        fetch(`${process.env.apiUrl}/constituencies`, {
            agent: httpsAgent,
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                'name': data.name,
                'parish': data.parish
            })
        })
            .then(res => {
                if (res.status === 201) {
                    setToast({
                        text: 'Successfully added!',
                        type: 'success'
                    })
                } else {
                    setToast({
                        text: 'Failed to add!',
                        type: 'error'
                    })
                }
            })
            .catch(error => {
                console.log(error)
                setToast({
                    text: 'Failed to add!',
                    type: 'error'
                })
            })
    }

    return (
        <DashboardLayout>
            <div className="map-container">
                <ConstituencyMap onClick={handleClick} />
                <div id="mapControls">
                    <Grid.Container gap={2}>
                        <Grid>
                            <Button type="secondary" icon={<Minus />} auto />
                        </Grid>
                        <Grid>
                            <Button type="secondary" icon={<Plus />} auto />
                        </Grid>
                    </Grid.Container>
                </div>
            </div>
            <div className="candidate-container">
                <Card shadow style={{height: '100%'}}>
                    <Row style={{ textAlign: 'center' }}>
                        <Col>
                            {
                                !member ? '' :
                                    <>
                                        <Avatar size="large" src={member.photo} />
                                        <h3>{member.firstName + ' ' + member.middleName + ' ' + member.lastName + ' ' + member.suffix}</h3>
                                    </>
                            }

                            <h4>{!mp ? '' : mp.ministerOf}</h4>

                            <h4>{!constituency ? '' : constituency.name}</h4>

                            {
                                !voters ? '' :
                                    <h5>{voters.length} Registered Voters</h5>
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
            {/* <Tabs initialValue="1">
                <Tabs.Item label="all" value="1">
                    
                </Tabs.Item>
                <Tabs.Item label="add" value="2">
                    <form>
                        <Grid.Container>
                            <Grid xl={6}>
                                <form onSubmit={handleSubmit(onSubmit)}>
                                    <Input name="name" ref={register({ required: true })} placeholder="Constituency name" size="large" width="100%">Name of Constituency</Input>
                                    <Spacer y={1} />
                                    <Select name="parish" onChange={handleParishChange} placeholder="Parish" size="large" width="100%">
                                        <Select.Option value="Hanover">Hanover</Select.Option>
                                        <Select.Option value="Kingston">Kingston</Select.Option>
                                        <Select.Option value="St. Catherine">St. Catherine</Select.Option>
                                        <Select.Option value="Clarendon">Clarendon</Select.Option>
                                        <Select.Option value="Manchester">Manchester</Select.Option>
                                        <Select.Option value="St. Elizabeth">St. Elizabeth</Select.Option>
                                        <Select.Option value="St. James">St. James</Select.Option>
                                        <Select.Option value="St. Andrew">St. Andrew</Select.Option>
                                        <Select.Option value="St. Thomas">St. Thomas</Select.Option>
                                        <Select.Option value="St. Ann">St. Ann</Select.Option>
                                        <Select.Option value="Westmoreland">Westmoreland</Select.Option>
                                        <Select.Option value="Portland">Portland</Select.Option>
                                        <Select.Option value="Trelawny">Trelawny</Select.Option>
                                        <Select.Option value="St. Mary">St. Mary</Select.Option>
                                    </Select>
                                    <Spacer y={1} />
                                    <Button htmlType="submit" type="secondary" ghost>Add</Button>
                                </form>
                            </Grid>
                        </Grid.Container>
                    </form>
                </Tabs.Item>
            </Tabs> */}
        </DashboardLayout>
    )
}

export async function getServerSideProps(context) {
    const cookies = parseCookies(context)

    const token = cookies.token
    const decodedToken = jwt.decode(token, { complete: true })
    var dateNow = moment(moment().valueOf()).unix()

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