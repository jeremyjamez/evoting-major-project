import PropTypes from 'prop-types';
import { Step, Steps } from 'react-step-builder';
import VerificationStepOne from "./verificationStepOne";
import {Row} from '@geist-ui/react';
import VerificationStepTwo from './verificationStepTwo';

function VerificationSteps() {

    return (
        <>
            <Row align="middle" style={{ height: '100%' }}>
                <Steps>
                    <Step component={VerificationStepOne} />
                    <Step component={VerificationStepTwo} />
                </Steps>
            </Row>
        </>
    )
}

VerificationSteps.propTypes = {
    onPageChanged: PropTypes.func
}

export async function getStaticProps() {
    const res = fetch("https://localhost:5000/api/Voters/{id}");
    
}

export default VerificationSteps;