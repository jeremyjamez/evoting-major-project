import { Grid, Card, Text } from "@geist-ui/react";
import DashboardLayout from "./layout";
import { Bar } from "@reactchartjs/react-chart.js";
import moment from "moment";
import nookies from 'nookies'
import jwt from 'jsonwebtoken'
import { useElections } from "../../utils/swr-utils";

export default function Home(props) {

    const { elections } = useElections(props.token)

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

    return (
        <DashboardLayout>
            <div style={{ margin: '16px' }}>
                <Grid.Container gap={2} >
                    <Grid xs={24} xl={24}>
                        <Card type="dark" hoverable>
                            {/*<h3>Next {elections[0].electionType} Election on {moment(elections[0].electionDate).format("DD/MM/YYYY hh:mm a")}</h3>*/}
                        </Card>
                    </Grid>
                    <Grid xs={24} xl={8}>
                        <Card shadow>
                            <h4>Registered Voters</h4>
                            <Bar data={voterPopulationData} />
                        </Card>
                    </Grid>
                    <Grid xs={24} xl={8}>
                        <Card shadow>
                            <h4>Vote Statistics <Text small>(per election)</Text></h4>
                        </Card>
                    </Grid>
                    <Grid xs={24} xl={8}>
                        <Card shadow>
                            <h4></h4>
                        </Card>
                    </Grid>

                    <Grid xl={8}>
                        <Card shadow>
                            <h4>Registered Polling Centres</h4>
                        </Card>
                    </Grid>
                    <Grid xl={8}>
                        <Card shadow>
                            <h4>Registered Polling Stations</h4>
                        </Card>
                    </Grid>
                    <Grid xl={8}>
                        <Card shadow>
                            <h4>Registered Polling Stations</h4>
                        </Card>
                    </Grid>

                    <Grid xs={24}>
                        {/* <Table data={data}>
                        <Table.Column prop={stationId} label="ID" />
                        <Table.Column prop={parish} label="Location" />
                        <Table.Column prop={divisionId} label="Division" />
                        <Table.Column prop={centreId} label="Centre" />
                        <Table.Column prop={status} label="Status" />
                    </Table> */}
                    </Grid>
                </Grid.Container>
            </div>
        </DashboardLayout>
    )
}

export async function getServerSideProps(ctx) {
    const cookies = nookies.get(ctx)

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