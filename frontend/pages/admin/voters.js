import React, { useState } from "react";
import { Grid } from "@geist-ui/react";
import DashboardLayout from "./layout";
import DataTable from "react-data-table-component";

export default function Voters() {

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

    return (
        <DashboardLayout>
            <Grid.Container style={{margin: '16px'}}>
                <Grid xl={24}>
                    <DataTable
                        noHeader
                        columns={columns}
                    />
                </Grid>
            </Grid.Container>
        </DashboardLayout>
    )
}