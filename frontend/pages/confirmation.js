import { Button, Grid, Page } from "@geist-ui/react"
import { useState } from "react";
import OtpInput from 'react-otp-input';

const inputStyle = {
    width: '60px',
    height: '60px',
    borderRadius: '8px',
    borderWidth: '1px',
    margin: '0 5px',
    fontSize: '2rem'
}

const focusStyle = {
    borderWidth: '2px'
}

const containerStyle = {
    margin: '50px 0'
}

const Confirmation = () => {
    const [otp, setOtp] = useState('')

    const handleChange = e => {
        setOtp(e)
    }
    return (
        <>
            <Page>
                <Grid.Container justify="center" gap={2}>
                    <Grid style={{textAlign: 'center'}}>
                        <h1>A confirmation code has been sent to the phone number you provided.</h1>
                        <h2>Enter it below and click Confirm to submit your vote.</h2>
                    </Grid>

                    <Grid>
                        <OtpInput
                            value={otp}
                            onChange={handleChange}
                            numInputs={6}
                            separator={<span>-</span>}
                            isInputNum
                            inputStyle={inputStyle}
                            containerStyle={containerStyle}
                            focusStyle={focusStyle}
                        />
                    </Grid>
                </Grid.Container>
                <Grid.Container justify="center">
                    <Grid>
                        <Button type="secondary" size="large" auto shadow>Confirm</Button>
                    </Grid>
                </Grid.Container>
            </Page>
        </>
    )
}

export default Confirmation