import Layout from "../components/layout"
import { parseCookies, destroyCookie, setCookie } from 'nookies'
import jwt from 'jsonwebtoken'
import moment from 'moment'
import { Grid, Text, Button, useToasts } from "@geist-ui/react"
import { useState } from "react"
import OtpInput from "react-otp-input"
import NodeRSA from 'node-rsa'
import https from "https"
import { useRouter } from "next/router"

const inputStyle = {
    borderRadius: '4px',
    width: '60px',
    height: '60px',
    margin: '8px',
    fontSize: '1.25rem',
    border: 'gray solid 1px'
}

const focusStyle = {
    border: '#2e2b2b solid 4px'
}

const Confirmation = ({ token, exp, public_key }) => {
    const [otp, setOtp] = useState('')
    const [,setToast] = useToasts()
    const router = useRouter()
    const handleOtpChange = e => setOtp(e)

    const confirmClick = () => {
        const cookies = parseCookies(null)
        const candidate = JSON.parse(cookies.candidate)

        const timeNow = moment().valueOf() / 1000

        const payload = {
            voterId: localStorage.getItem('voterId'),
            candidateId: candidate.candidateId,
            electionId: cookies.electionId,
            constituencyId: candidate.constituencyId,
            ballotTime: timeNow.toString()
        }

        var key = new NodeRSA(public_key)
        const encryptedPayload = key.encrypt(payload, 'base64')

        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });

        fetch(`${process.env.NEXT_PUBLIC_API_URL}/votes/${otp}`,
            {
                agent: httpsAgent,
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + token
                },
                body: JSON.stringify(encryptedPayload)
            })
            .then(res => {
                if (res.ok) {
                    res.json()
                        .then(t => {
                            if (!t) {
                                setToast({
                                    text: 'Confirmation failed. One-Time Password is invalid.',
                                    type: 'error'
                                })
                            } else {
                                setToast({
                                    text: 'Confirmation successful.',
                                    type: 'success'
                                })
                                destroyCookie(null, 'candidate')
                                setCookie(null, 'voteId', t)
                                router.push('/vote-result')
                            }
                        })
                }
            })
            .catch(error => {
                console.log(error)
            })
    }

    return (
        <Layout expireTimestamp={exp}>
            <Grid.Container gap={2} justify="center">
                <Grid xs={24}>
                    <Text h1>Enter your OTP below and click Confirm to submit your vote.</Text>
                </Grid>
                <Grid xs={24} justify="center">
                    <OtpInput value={otp}
                        onChange={handleOtpChange}
                        numInputs={6}
                        isInputNum={true}
                        isInputSecure={true}
                        shouldAutoFocus
                        inputStyle={inputStyle}
                        focusStyle={focusStyle} />
                </Grid>
                <Grid>
                    <Button type="secondary" size="large" disabled={otp.length < 6} onClick={confirmClick} shadow>Confirm</Button>
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
        return {
            props: {
                token,
                exp: tokenData.exp,
                public_key
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

export default Confirmation