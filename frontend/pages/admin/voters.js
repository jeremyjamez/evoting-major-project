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