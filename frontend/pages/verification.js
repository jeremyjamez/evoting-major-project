import { Col, Row, useCurrentState, Modal, Text, useModal } from "@geist-ui/react"
import { useEffect, useState } from "react"
import { Strider, Step } from "react-strider"
import GettingStarted from "../components/gettingStarted"
import Layout from "../components/layout"
import SecurityQuestion from "../components/SecurityQuestion"
import { parseCookies, destroyCookie } from 'nookies'
import jwt from 'jsonwebtoken'
import moment from 'moment'
import https from 'https'
import { useRouter } from "next/router"


const Verification = ({ questions, exp }) => {

    const [correct, setAnswerCorrect, correctValueRef] = useCurrentState(0)
    const [state, setState] = useState(false)
    const {visible, setVisible, bindings} = useModal()

    const router = useRouter()

    const pushAnswer = () => {
        setAnswerCorrect((prev) => prev + 1)
        if (correctValueRef.current === 2) {
            router.push('/selectCandidate')
        }
    }

    const pushAttempt = (attempt) => {
        if (attempt === 0) {
            setVisible(true)
        }
    }

    return (
        <Layout expireTimestamp={exp}>
            <Row align="middle" style={{ height: '100%' }}>
                <Col>
                    <Strider activeIndex="0">
                        <Step>
                            {({ next, goTo, active, hiding, activeIndex }) => (
                                <GettingStarted next={next} />
                            )}
                        </Step>
                        <Step>
                            {({ next, goTo, active, hiding, activeIndex }) => (
                                <SecurityQuestion triggerPushAnswer={pushAnswer} triggerPushAttempt={pushAttempt} number={activeIndex} item={questions[activeIndex - 1]} next={next} />
                            )}
                        </Step>
                        <Step>
                            {({ next, goTo, active, hiding, activeIndex }) => (
                                <SecurityQuestion triggerPushAnswer={pushAnswer} triggerPushAttempt={pushAttempt} number={activeIndex} item={questions[activeIndex - 1]} next={next} />
                            )}
                        </Step>
                    </Strider>
                </Col>
            </Row>
            <style global jsx>{`
                main {
                    height: 100vh
                }
            `}</style>
            <Modal {...bindings} disableBackdropClick={true}>
                <Modal.Title>
                    <Text h4>3 failed security question attempts</Text>
                </Modal.Title>
                <Modal.Content>
                    <Text h5>You have incorrectly answered the question using your 3 attempts. You will be redirected to the home page.</Text>
                </Modal.Content>
                <Modal.Action onClick={() => {
                    destroyCookie(null, 'token')
                    router.push('/')
                    }}>OK</Modal.Action>
            </Modal>
        </Layout>
    )
}

export async function getServerSideProps(context) {
    const cookies = parseCookies(context)
    const token = cookies.token

    const decodedToken = jwt.decode(token, { complete: true })
    const dateNow = moment(moment().valueOf()).unix()

    if (decodedToken !== null && decodedToken.payload.exp > dateNow) {

        const tokenData = decodedToken.payload

        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });

        const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/voters/getquestions/${tokenData.Id}`,
            {
                agent: httpsAgent,
                method: 'GET',
                headers: new Headers({
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + token
                })
            })

        if (res.ok) {
            const data = await res.json()
            const questions = data
            const exp = tokenData.exp
            return {
                props: {
                    questions,
                    exp
                }
            }
        }
    }

    return {
        redirect: {
            destination: '/',
            permanent: false
        }
    }
}

export default Verification