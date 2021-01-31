import { Button, Grid, Card, Row, Spacer, Tabs, Text, useTabs, Table } from "@geist-ui/react";
import DashboardLayout from "./layout";
import https from "https";
import { useEffect, useState } from "react";
import { Bar } from "@reactchartjs/react-chart.js";
import moment from "moment";

export default function Home({ data, electionData }) {

    /* const calculateTimeLeft = () => {
        const difference = +new Date(electionData.electionDate) - +new Date();
        let timeLeft = {};

        if (difference > 0) {
            timeLeft = {
                days: Math.floor(difference / (1000 * 60 * 60 * 24)),
                hours: Math.floor((difference / (1000 * 60 * 60)) % 24),
                minutes: Math.floor((difference / 1000 / 60) % 60),
                seconds: Math.floor((difference / 1000) % 60)
            };
        }
        return timeLeft;
    };

    const [timeLeft, setTimeLeft] = useState(calculateTimeLeft());

    useEffect(() => {
        const interval = setInterval(() => setTimeLeft(calculateTimeLeft), 1000);
        return () => {
            clearInterval(interval);
        };
    }, [])

    const timerComponents = [];

    Object.keys(timeLeft).forEach((interval) => {
        if (!timeLeft[interval]) {
            return;
        }

        timerComponents.push(
            <span>
                {timeLeft[interval]} {interval}{" "}
            </span>
        );
    }); */

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
            <Grid.Container gap={2}>
                <Grid xs={24} xl={24}>
                    <Card type="dark" hoverable>
                        <h3>Next {electionData.electionType} Election on {moment(electionData.electionDate).format("DD/MM/YYYY hh:mm a")}</h3>
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
        </DashboardLayout>
    )
}

export async function getStaticProps() {
    const httpsAgent = new https.Agent({
        rejectUnauthorized: false
    });

    const [data, electionData] = await Promise.all([
        fetch(`https://localhost:44387/api/PollingStations`, { agent: httpsAgent }).then(r => r.json()),
        fetch(`https://localhost:44387/api/Elections/2`, { agent: httpsAgent }).then(r => r.json())
    ]);

    return {
        props: {
            data,
            electionData
        }
    }
}