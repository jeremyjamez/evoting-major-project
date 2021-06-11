import { Grid, Spacer, Text } from "@geist-ui/react"
import Layout from "../components/layout"
import { useEffect, useState, useRef } from 'react'

const FacialVerification = () => {

    return (
        <Layout>
            <Grid.Container gap={2} justify="center">
                <Grid style={{ display: 'block' }}>
                    <WebcamVideo />
                    <Spacer y={2}/>
                    <Text h3>NOTE: Ensure your face is not covered or blocked and you are in a well lit area.</Text>
                </Grid>
            </Grid.Container>
        </Layout>
    )
}

export function WebcamVideo() {
    const [mediaStream, setMediaStream] = useState()
    const videoRef = useRef(null)

    useEffect(() => {
        async function setupWebcamVideo() {
            if (!mediaStream) {
                await setupMediaStream()
            } else {
                const videoCurr = videoRef.current
                if (!videoCurr) return
                const video = videoCurr
                if (!video.srcObject) {
                    video.srcObject = mediaStream
                }
            }
        }
        setupWebcamVideo()
    }, [mediaStream])

    async function setupMediaStream() {
        try {
            const ms = await navigator.mediaDevices.getUserMedia({
                video: { facingMode: "user" },
                audio: false
            })
            setMediaStream(ms)
        } catch (e) {
            alert("Camera is disabled");
            throw e
        }
    }
    return (
        <div className="camera-container">
            <video className="camera" ref={videoRef} autoPlay muted />
            <canvas id="overlay"></canvas>
            <style jsx>
                {`
                    .camera {
                        border-radius: 12px;
                        box-shadow: rgba(128, 128, 128, 0.3) 0px 14px 40px;
                        width: 80%;
                        margin: 0 auto;
                    }
                    .camera-container {
                        position: relative;
                        display: flex;
                    }
                    #overlay {
                        position: absolute;
                        top: 0;
                        left: 0;
                        width: 100%;
                        height: 100%;
                    }
                `}
            </style>
        </div>
    );
}

export default FacialVerification