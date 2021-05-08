import { Button, Grid, Input, Spacer, Text, useCurrentState } from "@geist-ui/react";

function camelPad(str) {
    return str
        // Look for long acronyms and filter out the last letter
        .replace(/([A-Z]+)([A-Z][a-z])/g, ' $1 $2')
        // Look for lower-case letters followed by upper-case letters
        .replace(/([a-z\d])([A-Z])/g, '$1 $2')
        // Look for lower-case letters followed by numbers
        .replace(/([a-zA-Z])(\d)/g, '$1 $2')
        .replace(/^./, function (str) { return str.toUpperCase(); })
        // Remove any white space left around the word
        .trim();
}

const SecurityQuestion = ({ item, pushAnswer, next, number }) => {

    const question = Object.keys(item)[0]

    const [answer, setAnswer] = useCurrentState('')

    const handleNext = () => {
        if (answer.toLowerCase() === item[question].toLowerCase()) {
            pushAnswer()
            next()
        } else {
            console.log('not correct')
        }
    }

    return (
        <>
            <Grid.Container gap={2} justify="center">
                <Grid xs={24}>
                    <Text style={{ textAlign: 'center' }} h1>What is your {camelPad(question)}?</Text>
                </Grid>
                <Grid xs={24}>
                    <Input style={{ fontSize: '1.5rem' }} onChange={(e) => setAnswer(e.target.value)} required placeholder="Type answer here" size="large" width="100%" status="secondary" clearable />
                </Grid>
            </Grid.Container>
            <Spacer y={1} />
            <Grid.Container justify="flex-end">
                <Grid>
                    <Button type="secondary" size="large" auto shadow onClick={handleNext}>Next</Button>
                </Grid>
            </Grid.Container>
        </>
    )
}

export default SecurityQuestion