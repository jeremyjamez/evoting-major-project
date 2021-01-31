import { Page } from '@geist-ui/react'
import { signIn, signOut, useSession } from 'next-auth/client'
import Login from './login'
import Layout from '../components/layout'

export default function Home() {
  const [session, loading] = useSession()
  return <>
    {!session && <>
      <Page dotBackdrop>
        <Page.Content>
          <Login />
        </Page.Content>
      </Page>
    </>}
    {session && <>

    </>}
  </>
}
