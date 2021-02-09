import { Component } from "react";
import { Button, Grid, Input, Spacer, Text } from "@geist-ui/react";

class SecurityQuestion extends Component {
    constructor(props) {
        super(props)
        this.state = {
            attempt: false
        }
    }

    handleAttempt(e){
        console.log(e)
    }

    handleNext() {
        this.state.attempt ? this.props.pushAnswer() : null
    }

    render() {

        const { item, number } = this.props
        const {question, answer} = item
        question = "What is your address?"
        return (
            <>
                <Grid.Container gap={2} justify="center">
                    <Grid xs={24}>
                        <Text style={{ textAlign: 'center' }} h1>{question}</Text>
                    </Grid>
                    <Grid xs={24}>
                        <Input style={{ fontSize: '1.5rem' }} onChange={this.handleAttempt} required placeholder="Type answer here" size="large" width="100%" status="secondary" clearable />
                    </Grid>
                </Grid.Container>
                <Spacer y={1} />
                <Grid.Container justify="flex-end">
                    <Grid>
                        <Button type="secondary" size="large" auto shadow onClick={() => this.props.next()}>Next</Button>
                    </Grid>
                </Grid.Container>
            </>
        )
    }
}

export default SecurityQuestion