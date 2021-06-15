import { Button, Card, Grid, Image, Input, Page, Row, Text } from '@geist-ui/react'
import { Archive } from '@geist-ui/react-icons'
import { useRouter } from 'next/router'
import { useState } from 'react'
import Link from 'next/link'

const ChooseOption = () => {
    const router = useRouter()

    return (
        <Page>
            <Grid.Container gap={8}>
                <Grid xs={12} style={{display: 'block', textAlign: 'center'}} justify="center">
                    <Link href="/pair">
                        <div className="option-btn">
                            <div className="option-content">
                                <div>
                                    <svg xmlns="http://www.w3.org/2000/svg" className="icon icon-tabler icon-tabler-2fa" width="24" height="24" viewBox="0 0 24 24" strokeWidth="2" stroke="currentColor" fill="none" strokeLinecap="round" strokeLinejoin="round">
                                        <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
                                        <path d="M7 16h-4l3.47 -4.66a2 2 0 1 0 -3.47 -1.54"></path>
                                        <path d="M10 16v-8h4"></path>
                                        <line x1="10" y1="12" x2="13" y2="12"></line>
                                        <path d="M17 16v-6a2 2 0 0 1 4 0v6"></path>
                                        <line x1="17" y1="13" x2="21" y2="13"></line>
                                    </svg>
                                </div>
                                <span>Setup 2FA</span>
                            </div>
                        </div>
                    </Link>
                    <Text h3>Choose this option if you have not yet setup 2 factor authentication (2FA) using either the Google Authenticator or Microsoft Authenticator App</Text>
                </Grid>
                <Grid xs={12} style={{display: 'block', textAlign: 'center'}} alignItems="center">
                    <Link href="/validatePin">
                        <div className="option-btn">
                            <div className="option-content">
                                <div>
                                    <Archive size="3rem" />
                                </div>
                                <span>Vote</span>
                            </div>
                        </div>
                    </Link>
                    <Text h3>Choose this option if you already have 2FA setup and the other requirements to vote.</Text>
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
        </Page>
    )
}

export default ChooseOption