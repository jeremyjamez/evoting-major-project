import { Button, Code, Grid, Loading, Page, Text } from "@geist-ui/react"
import { useRouter } from "next/router"
import { parseCookies } from "nookies"
import { usePair } from "../utils/swr-utils"

const Pair = () => {

    const router = useRouter()
    const cookies = parseCookies(null)
    const { qr, isLoading, isError } = usePair(cookies.voterId)

    return (
        <>
            <Page>
                <Text h2>Setting up your One-Time Password (OTP)</Text>
                <Grid.Container justify="center" gap={2}>
                    {
                        isLoading ? <Loading size="large" />
                            : <>
                                <Grid>
                                    <img src={qr.qr} width="350" height="350" />
                                </Grid>

                                <Grid>
                                    <Text h3>Scan the QR code above or manually enter the following code <Code>{qr.manualSetupCode}</Code> into the Authenticator application
                                    you have downloaded to setup your one-time password.</Text>

                                    <Text size="1.45rem">Google Authenticator or Microsoft Authenticator are recommended. They can be downloaded from the Play Store for Android
                                    or App Store for Apple devices.</Text>
                                </Grid>
                                <Grid>
                                    <Button type="secondary" shadow size="large" onClick={() => router.push('/choose-option')}>
                                        <Text h3>Done</Text>
                                    </Button>
                                </Grid>
                            </>
                    }
                </Grid.Container>
            </Page>
        </>
    )
}

export default Pair