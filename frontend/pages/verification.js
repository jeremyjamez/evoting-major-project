import { Button, Col, Grid, Input, Page, Row, Text } from "@geist-ui/react";
import VerificationSteps from "../components/verificationSteps";

export default function Verification() {
    return (
        <>
            <Page dotBackdrop>
                <Page.Content style={{ height: '100vh' }}>
                    <VerificationSteps/>
                </Page.Content>
            </Page>
        </>
    )
}