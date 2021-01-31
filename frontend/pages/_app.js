import '../styles/global.css'
import Head from 'next/head'
import { GeistProvider, CssBaseline } from '@geist-ui/react'
import { Provider } from 'next-auth/client'
import { SWRConfig } from 'swr'

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
    <SWRConfig
      value={{
        fetcher: (...args) => fetch(...args).then(res => res.json())
      }}
    >
      <Provider
        options={{
          keepAlive: 0
        }}
        session={pageProps.session}>
        <GeistProvider>
          <Head>
            <title>eVoting Prototype</title>
            <link rel="icon" href="/favicon.ico" />
          </Head>
          <CssBaseline />
          <Component {...pageProps} />
        </GeistProvider>
      </Provider>
    </SWRConfig>
  )
}

export default App
