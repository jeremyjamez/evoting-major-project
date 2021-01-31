import React, { useState, useMemo } from "react";
import { Button, Grid, Input, Select, Spacer, Spinner, Tabs, Tooltip, useToasts } from "@geist-ui/react";
import { Trash2 } from "@geist-ui/react-icons";
import DashboardLayout from "./layout";
import https from "https";
import DataTable from "react-data-table-component";
import moment from "moment";
import FilterComponent from "../../components/FilterComponent";
import { useElections } from "../../utils/swr-utils";

const Elections = () => {
    const [electionDate, setElectionDate] = useState();
    const [electionStartTime, setElectionStartTime] = useState();
    const [electionEndTime, setElectionEndTime] = useState();
    const [electionType, setElectionType] = useState();

    const { elections } = useElections('/Elections');

    const columns = React.useMemo(() => [
        {
            name: 'Election ID',
            selector: 'electionId',
        },
        {
            name: 'Election Type',
            selector: 'electionType'
        },
        {
            name: 'Election Date',
            selector: 'electionDate',
            format: row => moment(row.electionDate).format('DD/MM/YYYY')
        },
        {
            name: '',
            selector: '',
            cell: row => <Button icon={<Trash2 />} size="small" auto type="error" onClick={() => handleDelete(row.electionId)}></Button>
        }
    ], [])

    const [, setToast] = useToasts();

    const [filterText, setFilterText] = useState('');
    const [resetPaginationToggle, setResetPaginationToggle] = useState(false);

    const filteredElections = !elections ? null : elections.filter(item =>
        item.electionId === Number.parseInt(filterText) ||
        item.electionType && item.electionType.toLowerCase().includes(filterText.toLowerCase()) ||
        item.electionDate && item.electionDate.includes(filterText)
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

    async function addElectionClick() {
        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });
        fetch(`https://localhost:44387/api/Elections`, {
            agent: httpsAgent,
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                "electionType": electionType,
                "electionDate": moment(electionDate).toISOString()
            })
        })
            .then(response => response.status)
            .then(status => {
                if (status === 201) {
                    setToast({
                        text: 'Successfully saved to database!',
                        type: 'success'
                    });
                } else {
                    console.log(status)
                    setToast({
                        text: 'Failed to save to database!',
                        type: 'error'
                    });
                }
            })
            .catch(error => console.log(error))
    };

    const handleDelete = (id) => {
        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });
        fetch(`${process.env.apiUrl}/Elections/${id}`, {
            agent: httpsAgent,
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        })
            .then(res => res.status)
            .then(status => {
                if (status === 204) {
                    setToast({
                        text: 'Record removed successful!',
                        type: 'success'
                    });
                }
            })
            .catch(error => {
                console.log(error)
                setToast({
                    text: 'Failed to remove record!',
                    type: 'error'
                });
            })
    }

    const selectHandler = (val) => {
        setElectionType(val);
    };

    const electionDateHandler = (e) => {
        setElectionDate(e.target.value);
    };

    const electionTimeHandler = (e) => {
        setElectionTime(e.target.value);
    }

    return (
        <DashboardLayout>
            <Grid.Container gap={2}>
                <Grid xs={24}>
                    <Tabs initialValue="1">
                        <Tabs.Item label="all" value="1">
                            <DataTable
                                columns={columns}
                                data={filterText === '' ? elections : filteredElections}
                                pagination
                                highlightOnHover
                                subHeader
                                subHeaderComponent={subHeaderComponentMemo}
                            />
                        </Tabs.Item>
                        <Tabs.Item label="add" value="2">
                            <Grid.Container >
                                <Grid xs={24} xl={6} alignContent="center">
                                    <Grid.Container gap={2}>
                                        <Grid xs={24}>
                                            Election Type
                                            <Spacer y={.5} />
                                            <Select placeholder="Election Type" size="large" width="100%" onChange={selectHandler}>
                                                <Select.Option value="General Election">General Election</Select.Option>
                                                <Select.Option value="Local Government Election">Local Government Election</Select.Option>
                                                <Select.Option value="By-election">By-election</Select.Option>
                                            </Select>
                                        </Grid>
                                        <Grid xs={24}>
                                            <Input type="date" value={electionDate} onChange={electionDateHandler} size="large" width="100%" >
                                                Election Date
                                            </Input>
                                        </Grid>

                                        <Grid xs={12}>
                                            <Input type="time" value={electionStartTime} size="large" width="100%" >
                                                Start
                                            </Input>
                                        </Grid>
                                        <Grid xs={12}>
                                            <Input type="time" value={electionEndTime} size="large" width="100%" >
                                                End
                                            </Input>
                                        </Grid>
                                    </Grid.Container>

                                    <Spacer y={2} />

                                    <Button type="secondary" ghost onClick={addElectionClick}>Save</Button>
                                </Grid>
                            </Grid.Container>
                        </Tabs.Item>

                        <Tabs.Item label="update" value="3">

                        </Tabs.Item>
                    </Tabs>
                </Grid>
            </Grid.Container>
        </DashboardLayout>
    )
}

export default Elections;