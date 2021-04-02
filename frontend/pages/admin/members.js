import { Grid, Tabs, Select, Spacer, Input, Button, useToasts, Avatar, AutoComplete } from "@geist-ui/react"
import { useEffect, useMemo, useState } from "react"
import DataTable from "react-data-table-component"
import DashboardLayout from "./layout"
import https from "https";
import moment from "moment";
import { useMembers, usePoliticalParties } from "../../utils/swr-utils";
import FilterComponent from "../../components/FilterComponent"
import { useForm } from "react-hook-form"
import jwt from 'jsonwebtoken'
import { parseCookies } from 'nookies'

const Members = ({token}) => {
    const { members } = useMembers(token);
    const { parties } = usePoliticalParties(token);

    const columns = useMemo(() => [
        {
            name: 'Member ID',
            selector: 'memberId',
            sortable: true
        },
        {
            name: 'Photo',
            selector: 'photo',
            cell: row => (
                <Avatar src={row.photo} size="medium" />
            )
        },
        {
            name: 'Prefix',
            selector: 'prefix'
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
        {
            name: 'Suffix',
            selector: 'suffix'
        },
        {
            name: 'Address',
            selector: 'address'
        },
        {
            name: 'Telephone',
            selector: 'telephone'
        },
        {
            name: 'Date of Birth',
            selector: row => moment(row.dateofBirth).format("DD/MM/YYYY")
        },
        {
            name: 'Position',
            selector: 'position'
        },
        {
            name: 'Party ID',
            selector: 'partyId'
        },
        {
            name: 'Affiliation',
            selector: 'politicalParty.name'
        },
        {
            name: 'Member Since',
            selector: row => moment(row.joinDate).format("DD/MM/YYYY")
        }
    ]);

    const [filterText, setFilterText] = useState('');
    const [resetPaginationToggle, setResetPaginationToggle] = useState(false);

    const filteredMembers = filterText === '' ? members : members.filter(item =>
        item.memberId == Number.parseInt(filterText)
        || (item.firstName && item.firstName.toLowerCase().includes(filterText.toLowerCase()))
        || (item.middleName && item.middleName.toLowerCase().includes(filterText.toLowerCase()))
        || (item.lastName && item.lastName.toLowerCase().includes(filterText.toLowerCase()))
        || (item.position && item.position.toLowerCase().includes(filterText.toLowerCase()))
        || (item.politicalParty.name && item.politicalParty.name.toLowerCase().includes(filterText.toLowerCase()))
    );

    const subHeaderComponentMemo = useMemo(() => {
        const handleClear = () => {
            if (filterText) {
                setResetPaginationToggle(!resetPaginationToggle);
                setFilterText('');
            }
        };

        return <FilterComponent onFilter={e => setFilterText(e.target.value)} onClear={handleClear} filterText={filterText} />;
    }, [filterText, resetPaginationToggle]);

    const { register, setValue, handleSubmit } = useForm()

    const [telephone, setTelephone] = useState('')

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
        setValue("telephone", normalizeInput(e.target.value, telephone))
    };

    const handlePrefixChange = e => {
        setValue("prefix", e)
    }

    const handleSuffixChange = e => {
        setValue("suffix", e)
    }

    const handleAffiliationChange = e => {
        setValue("partyId", e)
    }

    const handleGenderChange = (e) => {
        setValue("gender", e)
    }

    const allOptions = [
        { label: 'Senator', value: 'Senator' },
        { label: 'Prime Minister', value: 'Prime Minister' },
        { label: 'Treasurer', value: 'Treasurer' },
    ]

    const [options, setOptions] = useState()
    const searchHandler = (currentValue) => {
        if (!currentValue) return setOptions([])
        const relatedOptions = allOptions.filter(item => item.value.toLowerCase().includes(currentValue.toLowerCase()))
        setOptions(relatedOptions)
        setValue("position", currentValue)
    }

    const [, setToast] = useToasts();

    useEffect(() => {
        register("prefix")
        register("suffix")
        register("telephone")
        register("partyId")
        register("gender")
        register("position")
    }, [register, register, register, register, register, register])

    const onSubmit = async (data) => {
        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });
        fetch(`${process.env.apiUrl}/api/members`, {
            agent: httpsAgent,
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                "prefix": data.prefix,
                "firstName": data.firstName,
                "middleName": data.middleName,
                "lastName": data.lastName,
                "suffix": data.suffix,
                "gender": data.gender,
                "address": data.address,
                "telephone": data.telephone,
                "dateofBirth": moment(data.dateofBirth).toISOString(),
                "partyId": Number.parseInt(data.partyId),
                "position": data.position,
                "joinDate": moment(data.joinDate).toISOString()
            })
        })
            .then(response => response.status)
            .then(status => {
                if (status === 201) {
                    setToast({
                        text: 'Successfully saved member to database!',
                        type: 'success'
                    });
                } else {
                    console.log(status)
                    setToast({
                        text: 'Failed to save member to database!',
                        type: 'error'
                    });
                }
            })
            .catch(error => {
                console.log(error)
                setToast({
                    text: 'Failed to save member to database!',
                    type: 'error'
                });
            })
    };

    return (
        <DashboardLayout>
            <Tabs initialValue="1" style={{margin: '16px'}}>
                {/**
                 * Tab that displays a list of all members
                 */}
                <Tabs.Item label="all" value="1">
                    <DataTable
                        columns={columns}
                        data={filteredMembers}
                        highlightOnHover
                        subHeader
                        subHeaderComponent={subHeaderComponentMemo}
                        noHeader
                    />
                </Tabs.Item>
                {/** 
                 * Add Members Tab 
                 * */}
                <Tabs.Item label="add" value="2">
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <Grid.Container gap={2} justify="center">
                            <Grid xl={3} style={{display:'block'}}>
                                Prefix
                                <Spacer y={.5} />
                                <Select placeholder="Mr/Mrs/Ms" size="large" name="prefix" onChange={handlePrefixChange} width="100%">
                                    <Select.Option value="Mr">Mr</Select.Option>
                                    <Select.Option value="Mrs">Mrs</Select.Option>
                                    <Select.Option value="Ms">Ms</Select.Option>
                                </Select>
                            </Grid>

                            <Grid xl={7}>
                                <Input placeholder="First Name" name="firstName" ref={register({ required: true })} width="100%" size="large">
                                    First Name
                                </Input>
                            </Grid>
                            <Grid xl={4}>
                                <Input placeholder="Middle Name" name="middleName" ref={register({ required: true })} width="100%" size="large">
                                    Middle Name
                                </Input>
                            </Grid>
                            <Grid xl={7}>
                                <Input placeholder="Last Name" name="lastName" ref={register({ required: true })} width="100%" size="large">
                                    Last Name
                                </Input>
                            </Grid>

                            <Grid xl={3} style={{display:'block'}}>
                                Suffix
                                <Spacer y={.5} />
                                <Select placeholder="Jr/Snr" name="suffix" onChange={handleSuffixChange} width="100%" size="large">
                                    <Select.Option value="">None</Select.Option>
                                    <Select.Option value="Jr">Jr</Select.Option>
                                    <Select.Option value="Snr">Snr</Select.Option>
                                    <Select.Option value="I">I</Select.Option>
                                    <Select.Option value="II">II</Select.Option>
                                    <Select.Option value="III">III</Select.Option>
                                </Select>
                            </Grid>

                            <Grid xl={3} style={{display:'block'}}>
                                Gender
                                <Spacer y={.5} />
                                <Select placeholder="Gender" name="gender" onChange={handleGenderChange} width="100%" size="large">
                                    <Select.Option value="Male">Male</Select.Option>
                                    <Select.Option value="Female">Female</Select.Option>
                                </Select>
                            </Grid>

                            <Grid xl={3}>
                                <Input placeholder="Date of Birth" name="dateofBirth" ref={register({ required: true })} type="date" width="100%" size="large">
                                    Date of Birth
                                </Input>
                            </Grid>

                            <Grid xl={3}>
                                <Input placeholder="Telephone" name="telephone" value={telephone} onChange={handleTelephoneChange} width="100%" size="large">
                                    Telephone
                                </Input>
                            </Grid>

                            <Grid xl={3} style={{display:'block'}}>
                                Position
                                <Spacer y={.5} />
                                <AutoComplete placeholder="Position" name="position" onSearch={searchHandler} width="100%" size="large" options={options}/>
                                {/* <Input placeholder="Position" name="position" autoComplete="organization-title" ref={register({required: true})} width="100%" size="large">
                                    Position
                                </Input> */}
                            </Grid>
                            <Grid xl={3} style={{display:'block'}}>
                                Affiliation
                                <Spacer y={.5} />
                                <Select placeholder="Affiliation" name="partyId" onChange={handleAffiliationChange} width="100%" size="large">
                                    {
                                        !parties ? null :
                                            parties.map(party => {
                                                return (
                                                    <Select.Option key={party.partyId} value={`${party.partyId}`}>{party.name}</Select.Option>
                                                )
                                            })
                                    }
                                </Select>
                            </Grid>

                            <Grid xl={3}>
                                <Input placeholder="Join Date" name="joinDate" ref={register} type="date" width="100%" size="large">
                                    Join Date
                                </Input>
                            </Grid>

                            <Grid xl={6}>
                                <Input placeholder="Address" name="address" ref={register({ required: true })} width="100%">Address</Input>
                            </Grid>

                            <Grid>
                                <Button htmlType="submit" type="secondary" ghost>Add Member</Button>
                            </Grid>
                        </Grid.Container>
                    </form>
                </Tabs.Item>
                <Tabs.Item label="update" value="3">
                    <Grid.Container>
                        <Grid>

                        </Grid>
                    </Grid.Container>
                </Tabs.Item>
            </Tabs>
        </DashboardLayout>
    )
}

export async function getServerSideProps(context){
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

export default Members