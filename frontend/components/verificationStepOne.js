import { Button, Grid, Input, Spacer, Text } from "@geist-ui/react";
import ChevronLeft from '@geist-ui/react-icons/chevronLeft';

/**
 * Component that displays the first security question for
 * verification purposes.
 * Hooked up to `verificationSteps.js` as a `Step` component
 * @param {*} props 
 */
export default function VerificationStepOne(props) {
    return (
        <>
            <Grid.Container gap={2} justify="center">
                <Grid xs={24}>
                    <Text style={{ textAlign: 'center' }} h1>What is your address?</Text>
                </Grid>
                <Grid xs={24}>
                    <Input style={{ fontSize: '1.5rem' }} required placeholder="Type answer here" size="large" width="100%" status="secondary" clearable />
                </Grid>
            </Grid.Container>
            <Spacer y={1}/>
            <Grid.Container justify="flex-end">
                <Grid>
                    <Button type="secondary" size="large" auto shadow>Next</Button>
                </Grid>
            </Grid.Container>
        </>
    )
}