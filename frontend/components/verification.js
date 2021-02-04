import PropTypes from 'prop-types';
import { Step, Steps } from 'react-step-builder';
import VerificationStepOne from "./verificationStepOne";
import {Row} from '@geist-ui/react';
import VerificationStepTwo from './verificationStepTwo';
import Stepper from 'react-stepper-horizontal'

/**
 * Component that displays the security questions.
 * Hooked up to `verificationSteps.js` as a `Step` component
 */
function Verification() {

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

export default Verification;