import { Button, Grid, Loading, Spacer, Text, useToasts } from "@geist-ui/react"
import { Camera } from '@geist-ui/react-icons'
import Layout from "../components/layout"
import { useEffect, useState, useRef, createRef, useCallback } from 'react'
import Webcam from 'react-webcam'
import { parseCookies } from "nookies"
import https from 'https'
import jwt from 'jsonwebtoken'
import moment from 'moment'
import { useRouter } from "next/router"
import NodeRSA from "node-rsa"

const FacialVerification = ({ exp }) => {
    const [verifying, setVerifying] = useState(false)

    if (verifying) {
        return <>
            <Layout expireTimestamp={exp}>
                <Loading size="large"><span style={{fontSize: '2rem'}}>Verifying Identity</span></Loading>
            </Layout>
        </>
    }

    return (
        <Layout expireTimestamp={exp}>
            <Grid.Container gap={2} justify="center">
                <Grid style={{ display: 'block' }}>
                    <WebcamVideo verifying={setVerifying} />
                </Grid>
            </Grid.Container>
        </Layout>
    )
}

export function WebcamVideo({ verifying }) {
    const cookies = parseCookies(null)

    const cameraStyle = {
        borderRadius: '12px',
        boxShadow: 'rgba(128, 128, 128, 0.3) 0px 14px 40px',
        width: '80%',
        margin: '0 auto'
    }

    const [,setToast] = useToasts()
    const router = useRouter()

    const videoConstraints = {
        facingMode: "user"
    };

    const webcamRef = useRef(null)

    const capture = useCallback(() => {
        const imgSrc = webcamRef.current.getScreenshot()

        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });

        var block = imgSrc.split(";");
                // Get the content type
                var contentType = block[0].split(":")[1];// In this case "image/jpeg"
                // get the real base64 content of the file
                var realData = block[1].split(",")[1];

        const payload = {
            voterId: cookies.voterId,
            photo: realData
        }

        //const key = new NodeRSA(cookies.public_key)
        //const encryptedPayload = key.encrypt(payload, 'base64')
        verifying(true)

        fetch(`${process.env.NEXT_PUBLIC_API_URL}/voters/verifyvoter`,
            {
                agent: httpsAgent,
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + cookies.token
                },
                body: JSON.stringify(payload)
            })
            .then(res => {
                if (res.ok) {
                    res.json()
                        .then(t => {
                            if (!t.isIdentical) {
                                setToast({
                                    text: 'Verification failed. Photos do not match.',
                                    type: 'error'
                                })
                                verifying(false)
                                router.push('/')
                            } else {
                                verifying(false)
                                setToast({
                                    text: 'Verification Successful.',
                                    type: 'success',
                                    delay: 2000
                                })
                                router.push('/selectCandidate')
                            }
                        })
                }
            })
            .catch(error => {
                console.log(error)
            })
    }, [webcamRef])

    const handleError = (e) => {
        if (e === "DOMException: Permission denied") {
            return <>
                <Text h1>Camera permission denied. <br /> Please allow access to the camera when requested by your browser.</Text>
                <Text h2>Refresh the page if the camera feed is not displayed after allowing access.</Text>
            </>
        }
    }

    return (
        <>
        <form id="form" hidden></form>
            <Text size="1.75rem" b>NOTE: Ensure your face is not covered or blocked and you are in a well lit area.</Text>
            <Spacer y={2}/>
            <div className="camera-container">
                <Webcam ref={webcamRef} audio={false} screenshotFormat="image/jpeg" videoConstraints={videoConstraints} style={cameraStyle} onUserMediaError={handleError} />
                <button onClick={capture}><Camera size={36} /></button>
                <style jsx>
                    {`
                    .camera-container {
                        position: relative;
                        display: flex;
                    }
                    #overlay {
                        position: absolute;
                        left: 50%;
                        bottom: -50%;
                        width: 80%;
                        height: 100%;
                        transform: translate(-50%, -50%);
                        border-radius: 12px;
                    }
                    button {
                        position: absolute;
                        left: 50%;
                        bottom: 12px;
                        transform: translate(-50%, -50%);
                        border-radius: 100%;
                        width: 70px;
                        height: 70px;
                        background-color: #e63946;
                        border: none;
                        color: white;
                    }

                    button:hover {
                        cursor: pointer;
                    }
                `}
                </style>
            </div>
        </>
    );
}

export async function getServerSideProps(context) {
    const cookies = parseCookies(context)
    const token = cookies.token

    const decodedToken = jwt.decode(token, { complete: true })
    const dateNow = moment(moment().valueOf()).unix()

    if (token !== null && decodedToken.payload.exp > dateNow) {

        const tokenData = decodedToken.payload
        const exp = tokenData.exp
        return {
            props: {
                exp
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
export default FacialVerification