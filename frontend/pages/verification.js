import { Col, Page, Row } from "@geist-ui/react"
import { Strider, Step } from "react-strider"
import GettingStarted from "../components/gettingStarted"
import PhoneNumber from "../components/phoneNumberStep"
import SecurityQuestion from "../components/SecurityQuestion"
import { useSecurityQuestions } from "../utils/swr-utils"

const Verification = (props) => {

    const { questions } = useSecurityQuestions(props.votersId)

    return (
        <>
            <Page style={{ height: '100%' }}>
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
                                    <SecurityQuestion {...props} number={activeIndex} next={next} />
                                )}
                            </Step>
                            <Step>
                                {({ next, goTo, active, hiding, activeIndex }) => (
                                    <SecurityQuestion {...props} number={activeIndex} next={next} />
                                )}
                            </Step>
                            <Step>
                                {({ next, goTo, active, hiding, activeIndex }) => (
                                    <PhoneNumber next={next}/>
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
            </Page>
        </>
    )
}

export default Verification