import { Button, Grid, Page, Text } from "@geist-ui/react"

const GettingStarted = (props) => {
    return (
        <>
            <Grid.Container>
                <Grid>
                    <Text h2>You will be asked two (2) security questions in order to verify your identity and your mobile phone number in order to receive
                        the confirmation code at the end.
                    </Text>
                    <Text type="warning" h3>Ensure that answers are spelt correctly and with correct formatting and that the phone number you enter is the one you currently use.</Text>
                </Grid>
            </Grid.Container>
            <Grid.Container justify="flex-end">
                <Grid>
                    <Button type="secondary" auto shadow onClick={() => props.next()}>Continue</Button>
                </Grid>
            </Grid.Container>
        </>
    )
}

export default GettingStarted