import { Page } from '@geist-ui/react'
import { signIn, signOut, useSession } from 'next-auth/client'
import Login from './login'
import Layout from '../components/layout'

export default function Home() {
  const [session, loading] = useSession()
  return <>
    {!session && <>
      <Page>
        <Page.Content>
          <Login />
        </Page.Content>
      </Page>
    </>}
    {session && <>
    {console.log(session)}
    <p>Brij</p>
    </>}
  </>
}
