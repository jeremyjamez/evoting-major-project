import { Button, Grid, Page, Spacer, Text } from "@geist-ui/react"
import { useState } from "react"
import OtpInput from 'react-otp-input'

export default function ValidatePin() {

    const [otp, setOtp] = useState('')

    const handleOtpChange = e => setOtp(e)

    return (
        <>
            <Page>
                <Text h2>Enter your One-Time Password (OTP) to continue</Text>
                <Grid.Container justify="center">
                    <Grid>
                        <OtpInput value={otp}
                            onChange={handleOtpChange}
                            numInputs={6}
                            isInputNum={true}
                            isInputSecure={true}
                            separator={<span>-</span>} />
                    </Grid>
                </Grid.Container>

                <Spacer y={1}/>

                <Grid.Container justify="center">
                    <Grid>
                        <Button type="secondary" shadow>Validate</Button>
                    </Grid>
                </Grid.Container>
            </Page>
        </>
    )
}