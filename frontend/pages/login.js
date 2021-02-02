import { Button, Card, Grid, Image, Input, Note, Row, Spacer, Text } from "@geist-ui/react"
import Link from 'next/link'

export default function Login() {
    return (
        <>
            <Grid.Container justify="center" gap={2}>
                <Grid xs={24} xl={12}>
                    <Card shadow>
                        <Row justify="center" style={{ marginBottom: '15px' }}>
                            <Image height={100} src="/images/electorial-logo.png" />
                        </Row>

                        <Row justify="center">
                            <Text h3 size="1.75rem" style={{ textAlign: 'center' }}>eVoting Prototype</Text>
                        </Row>

                        <Row style={{ marginBottom: '15px' }}>
                            <Input width="100%" size="large" clearable >Elector Registration No.</Input>
                        </Row>

                        <Row justify="center">
                            <Link href="/verification">
                                <Button type="secondary" size="large" ghost>Login</Button>
                            </Link>
                        </Row>
                    </Card>
                </Grid>
                <Grid xs={24}>
                    <Note filled type="error" style={{fontSize: '1.15rem'}}>No functionality. Click Login to move to next page.</Note>
                </Grid>
                <Grid xs={24}>
                    <Note filled style={{fontSize: '1.15rem'}}>Enter the <strong>Elector Registration No.</strong> that is located on your <strong>Voter's ID</strong> and click Login to continue.</Note>
                </Grid>
            </Grid.Container>
        </>
    )
}