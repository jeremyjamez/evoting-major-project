import { Avatar, Button, Card, Grid, Image, Page } from "@geist-ui/react"
import CandidateCard from "../components/CandidateCardComponent"
import Link from "next/link"
import Layout from "../components/layout"
import { parseCookies } from 'nookies'
import jwt from 'jsonwebtoken'
import moment from 'moment'
import https from 'https'

const candidates = [
    {

    }
]

const SelectCandidate = ({ exp }) => {
    return (
        <Layout expireTimestamp={exp}>
            <Grid.Container gap={2}>
                <Grid xs={24}>
                    <h1>Below are the candidates within your constituency</h1>
                </Grid>

                <Grid xs={24}>
                    <CandidateCard />
                </Grid>
            </Grid.Container>
            <Grid.Container justify="flex-end">
                <Grid>
                    <Link href="confirmation"><Button type="secondary" size="large" auto shadow>Next</Button></Link>
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

        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });

        return {
            props: {
                exp: tokenData.exp
            }
        }

        /* const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/voters/getquestions/${tokenData.Id}`,
            {
                agent: httpsAgent,
                method: 'GET',
                headers: new Headers({
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + token
                })
            })

        if (res.ok) {
            const data = await res.json()
            const questions = data
            const exp = tokenData.exp
            return {
                props: {
                    questions,
                    exp
                }
            }
        } */
    }

    return {
        redirect: {
            destination: '/',
            permanent: false
        }
    }

}

export default SelectCandidate