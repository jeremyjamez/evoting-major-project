import { Button, Grid, Text } from "@geist-ui/react"

const GettingStarted = (props) => {
    return (
        <>
            <Grid.Container>
                <Grid>
                    <Text h2>You will be asked two (2) questions in order to verify your identity.
                    </Text>
                    <Text type="error" h2>IMPORTANT: Ensure that your answers are spelt correctly.</Text>
                </Grid>
            </Grid.Container>
            <Grid.Container justify="flex-end">
                <Grid>
                    <Button type="secondary" shadow size="large" onClick={() => props.next()}>
                        <Text h3>Continue</Text>
                    </Button>
                </Grid>
            </Grid.Container>
        </>
    )
}

export default GettingStarted