import { Button, Code, Grid, Loading, Page, Text } from "@geist-ui/react"
import { useRouter } from "next/router"
import { usePair } from "../utils/swr-utils"

const Pair = () => {

    var router = useRouter()
    const { qr, isLoading, isError } = usePair(router.query.voterId)

    return (
        <>
            <Page>
                <Text h2>One-Time Password (OTP) Setup</Text>
                <Grid.Container justify="center" gap={2}>
                    {
                        isLoading ? <Loading size="large" />
                            : <>
                                <Grid>
                                    <img src={qr.qr} width="350" height="350" />
                                </Grid>

                                <Grid>
                                    <Text h4>Scan the QR code above or manually enter the following code <Code>{qr.manualSetupCode}</Code> into an Authenticator application
                                    of your choice to setup your one-time password.</Text>

                                    <Text size="1.25rem">Google Authenticator or Microsoft Authenticator are recommended. They can be downloaded from the Play Store for Android
                                    or App Store for iOS.
                                    </Text>
                                </Grid>
                                <Grid>
                                    <Button type="secondary" shadow onClick={() => router.push('/validatePin')}>Next</Button>
                                </Grid>
                            </>
                    }
                </Grid.Container>
            </Page>
        </>
    )
}

export default Pair