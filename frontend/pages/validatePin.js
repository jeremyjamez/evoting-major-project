import { Button, Grid, Page, Spacer, Text, useToasts } from "@geist-ui/react"
import { useRouter } from "next/router"
import { useState } from "react"
import OtpInput from 'react-otp-input'
import https from "https"
import { setCookie } from 'nookies'
import Layout from "../components/layout"

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

export default function ValidatePin() {

    const router = useRouter()
    const [, setToast] = useToasts()

    const [otp, setOtp] = useState('')

    const handleOtpChange = e => setOtp(e)

    const checkPin = () => {
        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });

        fetch(`${process.env.NEXT_PUBLIC_API_URL}/voters/validate`,
            {
                agent: httpsAgent,
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    pin: otp,
                    voterId: router.query.voterId
                })
            })
            .then(res => {
                if (res.ok) {
                    res.json()
                        .then(t => {
                            if (!t.isCorrect) {
                                setToast({
                                    text: 'Validation failed. One-Time Password is invalid.',
                                    type: 'error'
                                })
                            } else {
                                setToast({
                                    text: 'Validation successful.',
                                    type: 'success'
                                })
                                setCookie(null, 'token', t.token, {
                                    maxAge: 3600
                                })
                                router.push('/agreement')
                            }
                        })
                }
            })
            .catch(error => {
                console.log(error)
            })
    }

    return (
        <Layout>
            <Text h2>Enter your One-Time Password (OTP) to continue</Text>
            <Grid.Container justify="center" gap={2}>
                <Grid>
                    <OtpInput value={otp}
                        onChange={handleOtpChange}
                        numInputs={6}
                        isInputNum={true}
                        shouldAutoFocus
                        inputStyle={inputStyle}
                        focusStyle={focusStyle} />
                </Grid>
            </Grid.Container>

            <Spacer y={1} />

            <Grid.Container justify="center">
                <Grid>
                    <Button type="secondary" onClick={checkPin} disabled={otp.length < 6} shadow>Validate</Button>
                </Grid>
            </Grid.Container>
        </Layout>
    )
}