import '../styles/global.css'
import Head from 'next/head'
import { GeistProvider, CssBaseline } from '@geist-ui/react'

const myTheme = {
  "palette": {
    "selection": "#50aad9",
    "success": "#25d955",
    "successLight": "#13eb52",
    "successDark": "#adce26"
  }
}

function App({ Component, pageProps }) {

  return (
    <GeistProvider>
      <Head>
        <title>Online eVoting Prototype</title>
        <link rel="icon" href="/favicon.ico" />
      </Head>
      <CssBaseline />
      <Component {...pageProps} />
    </GeistProvider>
  )
}

export default App
