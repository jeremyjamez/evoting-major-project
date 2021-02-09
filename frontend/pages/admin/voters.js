import React, { useEffect, useState } from "react";
import { Grid, Input, Select, Spacer, Tabs, Button, useToasts, Radio } from "@geist-ui/react";
import DashboardLayout from "./layout";
import https from "https";
import DataTable from "react-data-table-component";
import { useVoters } from "../../utils/swr-utils";
import { useForm } from "react-hook-form";
import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import moment from "moment"

const schema = yup.object().shape({
    dateofBirth: yup.date().default(() => {
        return moment(this).toISOString()
    })
})

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

    const handleTelephoneChange = (e) => {
        setTelephone(normalizeInput(e.target.value, telephone))
    };

    const handlePrefix = e => {
        setValue("prefix", e)
    }

    const handleConstituency = e => {
        setValue("constituencyId", e)
    }

    const handleGender = e => {
        setValue("gender", e)
    }
    
    const handleParish = e => {
        setValue("parish", e)
    }

    const { register, errors, handleSubmit, setValue } = useForm({
        resolver: yupResolver(schema)
    })

    useEffect(() => {
        register("prefix")
        register("constituencyId")
        register("gender")
        register("parish")
    }, [register, register, register, register])

    const [, setToast] = useToasts()


    const onSubmit = async (data) => {
        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });

        fetch(`${process.env.apiUrl}/voters`, {
            agent: httpsAgent,
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: data
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
                <Tabs.Item label="Voters List" value="1">
                    <DataTable
                        columns={columns}
                        data={voters}
                    />
                </Tabs.Item>
                <Tabs.Item label="add" value="2">
                    <Spacer y={1} />
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <Grid.Container gap={2} justify="center">
                            <Grid xl={3}>
                                Prefix
                                <Spacer y={.5} />
                                <Select placeholder="Mr/Mrs/Ms" width="100%" size="large" onChange={handlePrefix}>
                                    <Select.Option value="Mr">Mr</Select.Option>
                                    <Select.Option value="Mrs">Mrs</Select.Option>
                                    <Select.Option value="Ms">Ms</Select.Option>
                                </Select>
                            </Grid>
                            <Grid xl={3}>
                                <Input name="firstName" ref={register} placeholder="First Name" width="100%" size="large" required>
                                    First Name
                                </Input>
                            </Grid>
                            <Grid xl={3}>
                                <Input name="middleName" ref={register} placeholder="Middle Name" width="100%" size="large" required>
                                    Middle Name
                                </Input>
                            </Grid>
                            <Grid xl={3}>
                                <Input name="lastName" ref={register} placeholder="Last Name" width="100%" size="large" required>
                                    Last Name
                                </Input>
                            </Grid>

                            <Grid xl={8}>
                                <Input name="address" ref={register} placeholder="Address" width="100%" size="large" required>
                                    Address
                                </Input>
                            </Grid>

                            <Grid xl={4}>
                                Parish
                                <Spacer y={.5}/>
                                <Select name="parish" onChange={handleParish} placeholder="Parish" size="large" width="100%">
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
                            </Grid>

                            <Grid xl={4}>
                                <Input name="dateofBirth" ref={register} type="date" width="100%" size="large" required>
                                    Date of Birth
                            </Input>
                            </Grid>

                            <Grid xl={5}>
                                <Input name="occupation" ref={register} placeholder="Occupation" width="100%" size="large" required>
                                    Occupation
                            </Input>
                            </Grid>

                            <Grid xl={5}>
                                <Input name="mothersMaidenName" ref={register} placeholder="Mother's maiden name" width="100%" size="large" required>
                                    Mother's maiden name
                            </Input>
                            </Grid>

                            <Grid xl={5}>
                                <Input name="telephone" ref={register} placeholder="Telephone" value={telephone} width="100%" size="large" onChange={handleTelephoneChange} required>
                                    Telephone
                            </Input>
                            </Grid>

                            <Grid xl={5}>
                                Constituency
                            <Spacer y={.5} />
                                <Select placeholder="Select Constituency" width="100%" size="large" onChange={handleConstituency}>
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
                                <Input name="placeofBirth" ref={register} placeholder="Place of birth" width="100%" size="large" required>
                                    Place of birth
                                </Input>
                            </Grid>

                            <Grid xl={5}>
                                <Input name="mothersPlaceofBirth" ref={register} placeholder="Mother's place of birth" width="100%" size="large" required>
                                    Mother's place of birth
                                </Input>
                            </Grid>

                            <Grid xl={5}>
                                <Input name="fathersPlaceofBirth" ref={register} placeholder="Father's place of birth" width="100%" size="large" required>
                                    Father's place of birth
                                </Input>
                            </Grid>
                            <Grid xl={5}>
                                Gender
                                <Spacer y={.5}/>
                                <Select placeholder="Gender" width="100%" size="large" onChange={handleGender}>
                                    <Select.Option value="Male">Male</Select.Option>
                                    <Select.Option value="Female">Female</Select.Option>
                                </Select>
                            </Grid>
                        </Grid.Container>
                        <Spacer y={2} />
                        <Grid.Container justify="center">
                            <Grid>
                                <Button htmlType="submit" type="secondary" shadow>Add</Button>
                            </Grid>
                        </Grid.Container>
                    </form>
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