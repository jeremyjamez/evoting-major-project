import React, { useState, useMemo } from "react";
import { Button, Grid, Input, Select, Spacer, Tabs, useToasts } from "@geist-ui/react";
import { Trash2 } from "@geist-ui/react-icons";
import DashboardLayout from "./layout";
import https from "https";
import DataTable from "react-data-table-component";
import moment from "moment";
import FilterComponent from "../../components/FilterComponent";
import { useElections } from "../../utils/swr-utils";
import jwt from 'jsonwebtoken'
import { parseCookies } from 'nookies'

const Elections = ({ token }) => {
    const [electionDate, setElectionDate] = useState();
    const [electionStartTime, setElectionStartTime] = useState();
    const [electionEndTime, setElectionEndTime] = useState();
    const [electionType, setElectionType] = useState();

    const { elections } = useElections(token);

    const columns = React.useMemo(() => [
        {
            name: 'Election ID',
            selector: 'id',
        },
        {
            name: 'Election Type',
            selector: 'type'
        },
        {
            name: 'Election Date',
            selector: 'date',
            format: row => moment(row.date * 1000).format('MM/DD/YYYY')
        },
        {
            name: 'Start Time',
            selector: 'startTime',
            format: row => moment(row.startTime * 1000).format("hh:mm a")
        },
        {
            name: 'End Time',
            selector: 'endTime',
            format: row => moment(row.endTime * 1000).format('hh:mm a')
        },
        {
            name: '',
            selector: '',
            cell: row => <Button icon={<Trash2 />} size="large" auto type="error" onClick={() => handleDelete(row.id)}></Button>
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

    return (
        <DashboardLayout>
            <Tabs initialValue="1" style={{ margin: '16px', width: '100%' }}>
                <Tabs.Item label="all" value="1">
                    <DataTable
                        columns={columns}
                        data={filterText === '' ? elections : filteredElections}
                        pagination
                        highlightOnHover
                        noHeader
                        subHeader
                        subHeaderComponent={subHeaderComponentMemo}
                    />
                </Tabs.Item>

                <Tabs.Item label="update" value="3">

                </Tabs.Item>
            </Tabs>
        </DashboardLayout>
    )
}

export async function getServerSideProps(context) {
    const cookies = parseCookies(context)

    const token = cookies.token
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

export default Elections;