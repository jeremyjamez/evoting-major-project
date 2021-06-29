import { Grid, Text } from "@geist-ui/react"
import crypto, { privateDecrypt } from "crypto"
import moment from "moment"
import { parseCookies } from "nookies"
import Layout from "../components/layout"
import jwt from 'jsonwebtoken'
import https from 'https'

const VoteResult = ({exp, voteDetails, electionTitle}) => {

    const date = voteDetails.ballotTime * 1000
    return (
        <Layout expireTimestamp={exp}>
            <Grid.Container>
                <Grid style={{display: 'block'}}>
                    <Text h1>Thank you for voting in the {electionTitle}.</Text>
                    <Text h3>The ID of your vote is {voteDetails.voteId}</Text>
                    <Text h3>Your ballot was cast {moment(date).format('DD/MM/YYYY hh:mm a')}</Text>
                </Grid>
            </Grid.Container>
        </Layout>
    )
}


export async function getServerSideProps(context) {
    const cookies = parseCookies(context)
    const token = cookies.token

    const decodedToken = jwt.decode(token, { complete: true })
    const dateNow = moment(moment().valueOf()).unix()

    if (decodedToken !== null && decodedToken.payload.exp > dateNow) {

        const tokenData = decodedToken.payload
        const public_key = cookies.public_key

        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });

        const voteDetails = JSON.parse(cookies.data)

        const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/elections/${voteDetails.electionId}`,
            {
                agent: httpsAgent,
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + token
                }
            })

        const data = await res.json()

        return {
            props: {
                exp: tokenData.exp,
                voteDetails,
                electionTitle: data.electionTitle
            }
        }
    }

    return {
        redirect: {
            destination: '/',
            permanent: false
        }
    }

}

export default VoteResult