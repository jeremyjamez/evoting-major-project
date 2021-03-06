import { Grid, Text } from '@geist-ui/react'
import { Archive } from '@geist-ui/react-icons'
import { useRouter } from 'next/router'
import Link from 'next/link'
import Layout from '../components/layout'

const ChooseOption = () => {
    const router = useRouter()

    return (
        <Layout>
            <Grid.Container gap={4}>
                <Grid xs={24}>
                    <Text h2>If you have already setup your One-Time Password, select the Vote button if you wish to vote. Otherwise, select the Exit button to come back later.</Text>
                </Grid>
                <Grid xs={24} md={12} lg={12} xl={12} style={{display: 'block', textAlign: 'center'}} alignItems="center">
                    <Link href="/validatePin">
                        <div className="option-btn">
                            <div className="option-content">
                                <h2>Vote</h2>
                            </div>
                        </div>
                    </Link>
                    <Text h3>Choose this option if you are ready to cast your vote and your device has a working front-facing camera.</Text>
                </Grid>
                <Grid xs={24} md={12} lg={12} xl={12} style={{display: 'block', textAlign: 'center'}} justify="center">
                    <Link href="/pair">
                        <div className="option-btn">
                            <div className="option-content">
                                <h2>Setup OTP</h2>
                            </div>
                        </div>
                    </Link>
                    <Text h3>Choose this option if the One-Time Password (OTP) was not added to the Google Authenticator or Microsoft Authenticator App. You can try adding it again.</Text>
                </Grid>
            </Grid.Container>
            <style jsx>
                {`
                    .option-btn {
                        width: 200px;
                        height: 200px;
                        background-color: white;
                        padding: 16px;
                        border-radius: 10px;
                        border: solid lightgrey 1px;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        margin: 0 auto 16px auto;
                    }

                    .option-btn:hover {
                        box-shadow: rgba(128, 128, 128, 0.1) 0px 14px 40px;
                        cursor: pointer;
                    }

                    .option-content {
                        text-align: center;
                    }

                    .option-content span {
                        font-size: 1.45rem;
                    }

                    .icon-tabler-2fa {
                        width: 3rem;
                        height: 3rem;
                    }
                `}
            </style>
        </Layout>
    )
}

export default ChooseOption