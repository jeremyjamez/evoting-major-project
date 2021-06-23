import { Button, Grid, useCurrentState, Text } from "@geist-ui/react"
import CandidateCard from "../components/CandidateCardComponent"
import Layout from "../components/layout"
import { parseCookies, setCookie } from 'nookies'
import jwt from 'jsonwebtoken'
import moment from 'moment'
import https from 'https'
import crypto, { privateDecrypt } from 'crypto'
import { useRouter } from "next/router"



const SelectCandidate = ({ exp, candidates, token }) => {

    const [selected, setSelected, selectedRef] = useCurrentState()
    const router = useRouter()

    const onNextClick = () => {
        if(selectedRef.current){
            setCookie(null, 'candidate', JSON.stringify(selectedRef.current))
            router.push('/confirmation')
        }
    }

    const handleCandidateSelect = (e) => {
        console.log(e)
    }

    return (
        <Layout expireTimestamp={exp}>
            <Grid.Container gap={2}>
                <Grid xs={24}>
                    <Text h2>Below are the {candidates.length} candidates for {candidates[0].constituencyName}. Choose ONLY ONE.</Text>
                </Grid>

                {
                    candidates.map((candidate, idx) => {
                        return <Grid xs={24} key={candidate.candidateId}>
                            <Grid.Container>
                                <Grid xs={20}>
                                    <CandidateCard candidate={candidate} token={token} selected={setSelected}/>
                                </Grid>
                                <Grid>
                                    <input type="checkbox" onChange={handleCandidateSelect(candidate)}></input>
                                </Grid>
                            </Grid.Container>
                        </Grid>
                    })
                }
            </Grid.Container>
            <Grid.Container justify="flex-end">
                <Grid>
                    <Button type="secondary" size="large" shadow onClick={onNextClick}>
                        <Text h3>Vote</Text>
                    </Button>
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

        const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/candidates/${tokenData.Id}`,
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

            /* const decryptedData = privateDecrypt({
                key: cookies.private_key,
                padding: crypto.constants.RSA_PKCS1_OAEP_PADDING,
                oaepHash: 'sha1',
                passphrase: `${process.env.NEXT_PUBLIC_PRIVATE_KEY_PASS}`
            }, Buffer.from(data, 'base64')) */

            const candidates = data

            return {
                props: {
                    candidates,
                    token,
                    exp: tokenData.exp
                }
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

export default SelectCandidate