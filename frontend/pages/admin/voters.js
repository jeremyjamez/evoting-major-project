import React, { useState } from "react"
import { Grid } from "@geist-ui/react"
import DashboardLayout from "./layout"
import DataTable from "react-data-table-component"
import { useVoters } from "../../utils/swr-utils"
import moment from "moment"
import nookies from 'nookies'
import jwt from 'jsonwebtoken'

export default function Voters({token}) {

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
        }

    ], []);

    const { voters, isLoading, isError } = useVoters(token)

    return (
        <DashboardLayout>
            <Grid.Container style={{margin: '16px'}}>
                <Grid xs={24} style={{display: 'block'}}>
                    <DataTable
                        noHeader
                        pagination
                        highlightOnHover
                        data={voters}
                        columns={columns}
                    />
                </Grid>
            </Grid.Container>
        </DashboardLayout>
    )
}

export async function getServerSideProps(ctx) {
    const cookies = nookies.get(ctx)

    const token = cookies.to
    const decodedToken = jwt.decode(token, { complete: true })
    var dateNow = moment(moment().valueOf()).unix()

    if(token == null){
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