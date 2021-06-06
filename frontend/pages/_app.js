import '../styles/global.css'
import Head from 'next/head'
import { GeistProvider, CssBaseline } from '@geist-ui/react'
if (typeof document === 'undefined') { //@ts-ignore 
  global.document = { querySelector: function () {}, }; }
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
        <title>Online E-Voting Prototype</title>
        <link rel="icon" href="/favicon.ico" />
      </Head>
      <CssBaseline />
      <Component {...pageProps} />
    </GeistProvider>
  )
}

export default App
