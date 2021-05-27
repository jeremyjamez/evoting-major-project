import { Button, Grid, Modal, Page, Text } from '@geist-ui/react'
import moment from 'moment'
import Countdown from 'react-countdown'
import { useRouter } from 'next/router'
import { useState } from 'react'
import { destroyCookie } from 'nookies'

const Layout = ({ children, expireTimestamp }) => {

    const router = useRouter()
    const [state, setState] = useState(false)

    const renderer = ({ minutes, seconds, completed }) => {
        if (completed) {
            setState(true)
            destroyCookie(null, 'token')
            // Render a completed state
            return <>
                <Modal open={state} onClose={() => setState(false)} disableBackdropClick={true}>
                    <Modal.Content>
                        <Text h5>Your session has expired. You may re-enter your information to continue if you were unable to submit your vote.</Text>
                    </Modal.Content>
                    <Modal.Action onClick={() => router.push('/')}>OK</Modal.Action>
                </Modal>
            </>
        } else {
            // Render a countdown
            return <Text h3>{minutes}:{seconds} remaining</Text>
        }
    }

    const handleExit = () => {
        destroyCookie(null, 'token')
        router.push('/')
    }

    return (
        <Page>
            <Grid.Container>
                <Grid xs={12} justify="flex-start">
                    <Button type="error-light" onClick={handleExit} auto size="large">Exit</Button>
                </Grid>
                <Grid xs={12} justify="flex-end">
                    {
                        expireTimestamp > 0 ? 
                        <Countdown 
                            date={moment.unix(expireTimestamp).format('DD MMM YYYY hh:mm a')} 
                            renderer={renderer} 
                            zeroPadTime={2}/> : null
                    }
                </Grid>
            </Grid.Container>
            {children}
        </Page>
    )
}

export default Layout