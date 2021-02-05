import { Button, Grid, Input, Spacer, Text } from "@geist-ui/react";

const PhoneNumber = (props) => {
    return (
        <>
            <Grid.Container gap={2} justify="center">
                <Grid xs={24}>
                    <Text style={{ textAlign: 'center' }} h1>Enter your phone number</Text>
                    <Text style={{ textAlign: 'center' }} h5>A confirmation code will be sent to this number in order to confirm your vote.</Text>
                </Grid>
                <Grid xs={24}>
                    <Input style={{ fontSize: '1.5rem' }} required placeholder="Phone number" size="large" width="100%" status="secondary" clearable />
                </Grid>
            </Grid.Container>
            <Spacer y={1} />
            <Grid.Container justify="flex-end">
                <Grid>
                    <Button type="secondary" size="large" auto shadow onClick={() => props.next()}>Next</Button>
                </Grid>
            </Grid.Container>
        </>
    )
}

export default PhoneNumber