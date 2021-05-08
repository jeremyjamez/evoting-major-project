import { Col, Row, useCurrentState } from "@geist-ui/react"
import { useEffect } from "react"
import { Strider, Step } from "react-strider"
import GettingStarted from "../components/gettingStarted"
import Layout from "../components/layout"
import SecurityQuestion from "../components/SecurityQuestion"
import { parseCookies } from 'nookies'
import jwt from 'jsonwebtoken'
import moment from 'moment'
import https from 'https'
import { useRouter } from "next/router"


const Verification = ({ questions, exp }) => {

    const [correct, setCorrect] = useCurrentState(0)
    const router = useRouter()

    useEffect(() => {
        if(correct === 2){
            router.push('/selectCandidate')
        }
    },[correct])

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
                                <SecurityQuestion pushAnswer={() => setCorrect(correct + 1)} number={activeIndex} item={questions[activeIndex - 1]} next={next} />
                            )}
                        </Step>
                        <Step>
                            {({ next, goTo, active, hiding, activeIndex }) => (
                                <SecurityQuestion pushAnswer={() => setCorrect(correct + 1)} number={activeIndex} item={questions[activeIndex - 1]} next={next} />
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