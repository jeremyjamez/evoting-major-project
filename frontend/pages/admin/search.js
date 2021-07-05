import { Grid, Page, Input } from "@geist-ui/react"
import DataTable from "react-data-table-component"
import { useGetVoterElection } from "../../utils/swr-utils"

const Search = ({token}) => {
    const columns = [
        {
            name: 'Voter ID',
            selector: 'voterId'
        },
        {
            name: 'Election ID',
            selector: 'electionId'
        },
        {
            name: 'Ballot Time',
            selector: row => row.ballotTime ? moment(row.ballotTime * 1000).format('DD/MM/YYYY hh:mm a') : ''
        }
    ]

    const { voters, isError, isLoading } = useGetVoterElection(token)

    return (
        <Page size="large">
            <Grid.Container>
                <Grid>
                    <Input>Search</Input>
                </Grid>
                <Grid xs={24}>
                    <DataTable
                    columns={columns}
                    data={voters}
                    pagination
                    noHeader />
                </Grid>
            </Grid.Container>
        </Page>
    )
}

export async function getServerSideProps(ctx) {
    const cookies = nookies.get(ctx)

    const token = cookies.to
    const decodedToken = jwt.decode(token, { complete: true })
    var dateNow = moment(moment().valueOf()).unix()

    if (decodedToken !== null && decodedToken.payload.exp > dateNow) {
        if(decodedToken.payload.role === 'EOJ' || decodedToken.payload.role === 'Administrator'){
            return {
                redirect: {
                    destination: '/admin/home',
                    permanent: false
                }
            }
        }

        if(decodedToken.payload.role === 'EDW'){
            return {
                props: {
                    token
                },
                redirect: {
                    destination: '/admin/search',
                    permanent: false
                }
            }
        }
    }

    destroyCookie(ctx, 'to', { path: '/' })

    return {
        redirect: {
            destination: '/admin/login',
            permanent: false
        }
    }
}

export default Search