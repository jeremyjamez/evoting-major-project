import { Button, Grid, Input, Text } from "@geist-ui/react";
import ChevronLeft from '@geist-ui/react-icons/chevronLeft';

const questions = [
    {
        id: 1,
        question: "What is your address?",
        answer: ""
    },
    {
        id: 2,
        question: "What is your occupation?",
        answer: ""
    }
];

export default function VerificationStepOne(props) {
    return (
        <>
            <Grid.Container gap={2} justify="center">
                <Grid xs={24}>
                    <Text style={{ textAlign: 'center' }} h1>{questions[0].question}</Text>
                </Grid>
                <Grid xs={24}>
                    <Input style={{ fontSize: '1.5rem' }} required placeholder="Type answer here" size="large" width="100%" status="secondary" clearable />
                </Grid>

                <Grid xs={24}>
                    <Grid.Container justify="space-between">
                        <Grid><Button iconRight={<ChevronLeft />} type="secondary" effect ghost size="large" disabled={props.isFirst()} auto></Button></Grid>
                        <Grid><Button size="large" type="success" disabled={props.isLast()} auto effect shadow>Next</Button></Grid>
                    </Grid.Container>
                </Grid>
            </Grid.Container>
        </>
    )
}