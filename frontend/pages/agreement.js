import { Button, Grid, Page, Radio, Text, Spacer } from "@geist-ui/react";
import Link from "next/link";

export default function Agreement() {
    return (
        <>
            <Page>
                <Grid.Container>
                    <Grid>
                        <h1>Please take note of the following.</h1>
                        <Text size="1.35rem">
                            In order to cast your voting using the online e-voting platform, you will be required to answer security questions in order to verify your identity
                            and your mobile phone number where you will receive a confirmation code upon completion to confirm your vote. Your identity will remain confidential
                            at all times and votes and other confidential information will be stored in an encrypted database.
                            <Spacer y={.5}/>
                            Below are a list of the rules to which all voters must adhere to.
                        </Text>
                        <ol>
                            <li>Do not photograph, screenshot, screen record nor copy your vote.</li>
                            <li>Do not record the voting process.</li>
                            <li>Do not share your vote with anyone on social media (WhatsApp, Facebook, Instagram, etc) or in person.</li>
                            <li>Ensure you are in a space where the screen of your device cannot be seen by anyone while casting your vote.</li>
                            <li>Lorem Ipsum</li>
                        </ol>

                        <Spacer y={1}/>

                        <Text type="error" size="1.2rem" b>
                            Failure to comply with the above rules is a Breach of Secrecy and will result in your vote being cancelled if found guilty.
                        </Text>

                        <Spacer y={1}/>

                        <Radio.Group size="large" useRow>
                            <Radio value="1">I don't agree</Radio>
                            <Radio value="2">I Agree</Radio>
                        </Radio.Group>
                    </Grid>
                </Grid.Container>
                <Grid.Container justify="flex-end">
                    <Grid>
                        <Link href="/verification"><Button type="secondary" size="large" auto shadow>Continue</Button></Link>
                    </Grid>
                </Grid.Container>
            </Page>
        </>
    )
}