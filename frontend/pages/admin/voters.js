import React, { useState } from "react";
import { Grid, Input, Select, Spacer, Tabs, Button } from "@geist-ui/react";
import DashboardLayout from "./layout";
import https from "https";
import DataTable from "react-data-table-component";
import { Save } from "@geist-ui/react-icons";
import { useVoters } from "../../utils/swr-utils";

export default function Voters({ constituencyData }) {
    const voters = useVoters('/Voters');
    const columns = React.useMemo(() => [
        {
            name: 'Voter ID',
            selector: 'voterId'
        },
        {
            name: 'First Name',
            selector: 'firstName'
        },
        {
            name: 'Middle Name',
            selector: 'middleName'
        },
        {
            name: 'Last Name',
            selector: 'lastName'
        },

    ], []);

    const [telephone, setTelephone] = useState('');

    const normalizeInput = (value, previousValue) => {
        if (!value) return value;
        const currentValue = value.replace(/[^\d]/g, '');
        const cvLength = currentValue.length;

        if (!previousValue || value.length > previousValue.length) {
            if (cvLength < 4) return currentValue;
            if (cvLength < 7) return `(${currentValue.slice(0, 3)}) ${currentValue.slice(3)}`;
            return `(${currentValue.slice(0, 3)}) ${currentValue.slice(3, 6)}-${currentValue.slice(6, 10)}`;
        }
    };

    const handleTelephoneChange = (e) =>{
        setTelephone(normalizeInput(e.target.value, telephone))
    };

    return (
        <DashboardLayout>
            <Tabs initialValue="1">
                <Tabs.Item label="Voters List" value="1">
                    <DataTable
                        columns={columns}
                        data={voters}
                    />
                </Tabs.Item>
                <Tabs.Item label="add" value="2">
                    <Spacer y={1} />
                    <Grid.Container gap={2} justify="center">
                        <Grid xl={3}>
                            Prefix
                            <Spacer y={.5} />
                            <Select placeholder="Mr/Mrs/Ms" width="100%" size="large">
                                <Select.Option value="Mr">Mr</Select.Option>
                                <Select.Option value="Mrs">Mrs</Select.Option>
                                <Select.Option value="Ms">Ms</Select.Option>
                            </Select>
                        </Grid>
                        <Grid xl={3}>
                            <Input placeholder="First Name" width="100%" size="large">
                                First Name
                            </Input>
                        </Grid>
                        <Grid xl={3}>
                            <Input placeholder="Middle Name" width="100%" size="large">
                                Middle Name
                            </Input>
                        </Grid>
                        <Grid xl={3}>
                            <Input placeholder="Last Name" width="100%" size="large">
                                Last Name
                            </Input>
                        </Grid>

                        <Grid xl={12}>
                            <Input placeholder="Address" width="100%" size="large">
                                Address
                            </Input>
                        </Grid>

                        <Grid xl={4}>
                            <Input type="date" width="100%" size="large">
                                Date of Birth
                            </Input>
                        </Grid>

                        <Grid xl={5}>
                            <Input placeholder="Occupation" width="100%" size="large">
                                Occupation
                            </Input>
                        </Grid>

                        <Grid xl={5}>
                            <Input placeholder="Mother's maiden name" width="100%" size="large">
                                Mother's maiden name
                            </Input>
                        </Grid>

                        <Grid xl={5}>
                            <Input placeholder="Telephone" value={telephone} width="100%" size="large" onChange={handleTelephoneChange} required>
                                Telephone
                            </Input>
                        </Grid>

                        <Grid xl={5}>
                            Constituency
                            <Spacer y={.5} />
                            <Select placeholder="Select Constituency" width="100%" size="large">
                                {
                                    constituencyData.map(constituency => {
                                        return (
                                            <Select.Option key={`${constituency.constituencyId}`} value={`${constituency.constituencyId}`}>{constituency.name}</Select.Option>
                                        )
                                    })
                                }
                            </Select>
                        </Grid>

                        <Grid xl={5}>
                            <Input placeholder="Place of birth" width="100%" size="large">
                                Place of birth
                            </Input>
                        </Grid>

                        <Grid xl={5}>
                            <Input placeholder="Mother's place of birth" width="100%" size="large">
                                Mother's place of birth
                            </Input>
                        </Grid>

                        <Grid xl={5}>
                            <Input placeholder="Father's place of birth" width="100%" size="large">
                                Father's place of birth
                            </Input>
                        </Grid>

                        <Grid>
                            <Button icon={<Save/>} type="secondary" ghost>Save</Button>
                        </Grid>
                    </Grid.Container>
                </Tabs.Item>
                <Tabs.Item label="update" value="3"></Tabs.Item>
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