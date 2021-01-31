import Footer from "./footer"
import Head from 'next/head'
import { Page } from '@geist-ui/react'

export default function Layout({ children }) {
    return (
        <>
            <Page>
                <Head>
                    <title>eVoting</title>
                    <link rel="icon" href="/favicon.ico" />
                </Head>

                <Page.Content>
                    {{ children }}
                </Page.Content>
                <Footer />
            </Page>
        </>
    )
}