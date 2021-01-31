import React, { useEffect, useState, useMemo } from "react";
import { Tabs, Grid, Text, Collapse, Pagination, Spacer, Input, Select, Button, Loading, AutoComplete } from "@geist-ui/react";
import DashboardLayout from "./layout";
import https from "https";
import moment from "moment";
import DataTable from "react-data-table-component";
import FilterComponent from "../../components/FilterComponent";
import { useForm } from "react-hook-form";
import { useCandidates, useMembers } from "../../utils/swr-utils";

export default function Candidates({ constituencyData, electionData }) {
    const { candidates } = useCandidates('/candidates');
    const { members } = useMembers('/members');

    var [selectedRows, setSelectedRows] = useState([]);

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
                name: 'Constituency',
                selector: 'constituency.name',
            },
            {
                name: 'Full Name',
                selector: (val) => {
                    return val.member.firstName + " " + val.member.middleName + " " + val.member.lastName
                }
            },
            {
                name: 'Affiliation',
                selector: 'member.politicalParty.name'
            },
            {
                name: 'Position',
                selector: 'member.position',
            },
            {
                name: 'Election ID',
                selector: 'electionId'
            },
            {
                name: 'Election Date',
                selector: d => {
                    return moment(d.election.electionDate)
                        .local()
                        .format("DD/MM/YYYY hh:mm a")
                }
            },
        ], []);

    const memberCol = useMemo(
        () => [
            {
                name: 'Member ID',
                selector: 'memberId',
                right: true
            },
            {
                name: 'Prefix',
                selector: 'prefix',
            },
            {
                name: 'Full Name',
                selector: (val) => {
                    return val.firstName + " " + val.middleName + " " + val.lastName
                }
            },
            {
                name: 'Suffix',
                selector: 'suffix',
            },
            {
                name: 'Affiliation',
                selector: 'politicalParty.name',
            },
            {
                name: 'Position',
                selector: 'position',
            },
            {
                name: 'Member Since',
                selector: d => {
                    return moment(d.joinDate)
                        .local()
                        .format("DD/MM/YYYY hh:mm a");
                },
            },
        ],
        []
    );

    const [filterText, setFilterText] = useState('');
    const [resetPaginationToggle, setResetPaginationToggle] = useState(false);


    const filteredCandidates = filterText === '' ? (candidates) : candidates.filter(item =>
        item.candidateId == Number.parseInt(filterText)
        || (item.member.firstName && item.member.firstName.toLowerCase().includes(filterText.toLowerCase()))
        || (item.member.middleName && item.member.middleName.toLowerCase().includes(filterText.toLowerCase()))
        || (item.member.lastName && item.member.lastName.toLowerCase().includes(filterText.toLowerCase()))
        || (item.member.position && item.member.position.toLowerCase().includes(filterText.toLowerCase()))
        || item.constituencyId == Number.parseInt(filterText)
        || (item.constituency.name && item.constituency.name.toLowerCase().includes(filterText.toLowerCase()))
        || (item.member.politicalParty.name && item.member.politicalParty.name.toLowerCase().includes(filterText.toLowerCase()))
        || item.electionId == Number.parseInt(filterText)
    );

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

    useEffect(() => {
    }, [JSON.stringify(selectedRows)]);

    const handleChange = (state) => {
        setSelectedRows(state.selectedRows);
    }

    const handleConstituencyChange = (val) => {

    }

    const { register, errors, handleSubmit, setValue } = useForm()
    const onSubmit = (data) => {

    }

    const allOptions = [
        { label: 'Prime Minister', value: 'Prime Minister' },
        { label: 'Education, Youth and Information', value: 'Education, Youth and Information' },
        { label: 'Health and Wellness', value: 'Health and Wellness' },
        { label: 'Science, Energy and Technology', value: 'Science, Energy and Technology' },
        { label: 'Justice', value: 'Justice' },
        { label: 'Industry, Investment and Commerce', value: 'Industry, Investment and Commerce' },
        { label: 'Local Government and Rural Development', value: 'Local Government and Rural Development' },
        { label: 'Tourism', value: 'Tourism' },
        { label: 'Agriculture and Fisheries', value: 'Agriculture and Fisheries' },
        { label: 'National Security', value: 'National Security' },
        { label: 'Foreign Affairs and Foreign Trade', value: 'Foreign Affairs and Foreign Trade' },
        { label: 'Labour and Social Security', value: 'Labour and Social Security' },
        { label: 'Finance and the Public Service', value: 'Finance and the Public Service' },
        { label: 'Culture, Gender, Entertainment and Sport', value: 'Culture, Gender, Entertainment and Sport' },
        { label: 'Housing, Urban Renewal, Environment and Climate Change', value: 'Housing, Urban Renewal, Environment and Climate Change' },
        { label: 'Transport and Mining', value: 'Transport and Mining' },
    ]

    const [options, setOptions] = useState()
    const searchHandler = (currentValue) => {
        if (!currentValue) return setOptions([])
        const relatedOptions = allOptions.filter(item => item.value.toLowerCase().includes(currentValue.toLowerCase()))
        setOptions(relatedOptions)
        setValue("ministerOf", currentValue)
    }

    return (
        <DashboardLayout>
            <Tabs initialValue="1">
                <Tabs.Item label="all candidates" value="1">
                    {
                        !candidates ? <Loading size="large">Loading</Loading> :
                            <DataTable
                                columns={candidateCol}
                                data={filteredCandidates}
                                highlightOnHover
                                fixedHeader
                                pagination
                                paginationResetDefaultPage={resetPaginationToggle}
                                subHeader
                                subHeaderComponent={subHeaderComponentMemo} />
                    }
                </Tabs.Item>
                <Tabs.Item label="add" value="2">
                    <DataTable
                        columns={memberCol}
                        data={filteredMembers}
                        highlightOnHover
                        selectableRows
                        selectableRowsHighlight
                        onSelectedRowsChange={handleChange}
                        fixedHeader
                        pagination
                        paginationResetDefaultPage={resetPaginationToggle}
                        subHeader
                        subHeaderComponent={subHeaderComponentMemo} />

                    <Spacer y={2} />

                    <Grid.Container style={{ padding: '16px' }}>
                        <Grid xs={24}>
                            <Collapse.Group>
                                {
                                    selectedRows.map((row) => {
                                        return (
                                            <>
                                                <Collapse title={`${row.firstName} ${row.lastName}`} subtitle={`${row.politicalParty.name} - ${row.position}`}>
                                                    <form onSubmit={handleSubmit(onSubmit)}>
                                                        <Grid.Container gap={2}>
                                                            <Grid xs={8}>
                                                                <Select name="constituencyId" placeholder="Choose constituency" onChange={handleConstituencyChange} size="large" width="100%">
                                                                    {
                                                                        constituencyData.map(constituency => {
                                                                            return (
                                                                                <Select.Option value={`${constituency.constituencyId}`}>{constituency.name}</Select.Option>
                                                                            )
                                                                        })
                                                                    }
                                                                </Select>

                                                                <Spacer x={2} />

                                                                <Select name="electionId" placeholder="Select election" size="large" width="100%">
                                                                    {
                                                                        electionData.map(election => {
                                                                            return (
                                                                                <Select.Option
                                                                                    value={`${election.electionId}`}>
                                                                                    {election.electionType} - {moment(election.electionDate).format("DD/MM/YYYY")}
                                                                                </Select.Option>
                                                                            )
                                                                        })
                                                                    }
                                                                </Select>

                                                                <Spacer x={2} />

                                                                <AutoComplete name="ministerOf" placeholder="Ministry" onSearch={searchHandler} options={options} size="large" width="100%"/>
                                                            </Grid>
                                                            <Grid xs={24}>
                                                                <Button htmlType="submit" type="secondary">Add as candidate</Button>
                                                            </Grid>
                                                        </Grid.Container>
                                                    </form>
                                                </Collapse>
                                            </>
                                        )
                                    })
                                }
                            </Collapse.Group>
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
}