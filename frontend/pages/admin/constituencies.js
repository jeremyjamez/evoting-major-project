import { Button, Grid, Input, Select, Spacer, Tabs, Row, Col, Avatar } from "@geist-ui/react"
import { useEffect, useState } from "react"
import ConstituencyMap from "../../components/ConstituencyMap"
import { useConstituencies, useConstituencyMP } from "../../utils/swr-utils"
import DashboardLayout from "./layout"
import https from "https"
import { useForm } from "react-hook-form"
import { Minus, Plus } from "@geist-ui/react-icons"

const Constituencies = ({ constituencyData }) => {

    const [mapCol, setMapCol] = useState(24)
    const [sideCol, setSideCol] = useState(0)
    const [constituencyName, setConstituencyName] = useState()

    const handleClick = (name) => {
        setConstituencyName(name)
    }

    const handleChangeColSize = (colSize) => {
        setSideCol(colSize)
        setMapCol(colSize === 4 ? 20 : 24)
    }

    const { constituency, candidate, mp, member, voters } = useConstituencyMP(constituencyName)
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
            <Tabs initialValue="1">
                <Tabs.Item label="all" value="1">
                    <Grid.Container justify="center">
                        <Grid xl={mapCol}>
                            <ConstituencyMap colsize={handleChangeColSize} onClick={handleClick} />
                        </Grid>
                        <Grid xl={sideCol} style={{ borderLeft: '2px solid #f5f5f5', padding: '8px' }}>
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
                        </Grid>
                    </Grid.Container>
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
                    <style jsx>{`
                            #mapControls {
                                position: absolute;
                                bottom: 5%;
                            }
                            `}</style>
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
            </Tabs>
        </DashboardLayout>
    )
}

export async function getStaticProps() {
    const httpsAgent = new https.Agent({
        rejectUnauthorized: false,
    });

    const [constituencyData] = await Promise.all([
        fetch(`${process.env.apiUrl}/Constituencies`, { agent: httpsAgent }).then(r => r.json())
    ]);

    return {
        props: {
            constituencyData
        }
    }
}

export default Constituencies;