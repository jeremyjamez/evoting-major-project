import { Button, Card, Grid, Image, Input, Row, Spacer, Text } from "@geist-ui/react"
import Link from 'next/link'

export default function Login() {
    return (
        <>
            <Row justify="center">
                <Card width="500px" shadow>
                    <Row justify="center" style={{ marginBottom: '15px' }}>
                        <Image height={100} src="/images/electorial-logo.png" />
                    </Row>

                    <Row justify="center">
                        <Text h3 size="1.75rem" style={{ textAlign: 'center' }}>Electoral Commission of Jamaica</Text>
                    </Row>

                    <Row>
                        <Text p size="1.55rem" style={{ textAlign: 'center' }} >
                            Enter the <Text size="1.75rem" b>Elector Registration No.</Text> that is located on your <Text size="1.75rem" b>Voter's ID</Text> and click Login to continue.
                        </Text>
                    </Row>

                    <Row style={{ marginBottom: '15px' }}>
                        <Input placeholder="Voter ID" width="100%" size="large" clearable />
                    </Row>

                    <Row justify="center">
                        <Link href="/verification">
                            <Button type="secondary" size="large" ghost>Login</Button>
                        </Link>
                    </Row>
                </Card>
            </Row>
        </>
    )
}