import { Button, Grid, Loading, Spacer, Text } from "@geist-ui/react"
import { Camera } from '@geist-ui/react-icons'
import Layout from "../components/layout"
import { useEffect, useState, useRef } from 'react'

const FacialVerification = () => {

    return (
        <Layout>
            <Grid.Container gap={2} justify="center">
                <Grid style={{ display: 'block' }}>
                    <WebcamVideo />
                </Grid>
            </Grid.Container>
        </Layout>
    )
}

export function WebcamVideo() {
    const [mediaStream, setMediaStream] = useState()
    const videoRef = useRef(null)
    let canvas = document.createElement('canvas')
    
    let camera = document.querySelector('.camera')

    canvas.width = camera.videoWidth
    canvas.height = camera.videoHeight

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
            //alert("Camera is disabled");
            throw e
        }
    }

    const capture = () => {
        canvas.getContext('2d').drawImage(camera, 0, 0, camera.videoWidth, camera.videoHeight);
   	    let image_data_url = canvas.toDataURL('image/jpeg');

   	    // data url of the image
   	    console.log(image_data_url);
    }

    if(!mediaStream) {
        return <>
        <Text h1>Camera blocked. <br/> Please allow access to the camera when requested by your browser.</Text>
        <Text h2>Refresh the page if the camera feed is not displayed after allowing access.</Text>
        </>
    }

    return (
        <>
        <div className="camera-container">
            <video className="camera" ref={videoRef} autoPlay muted />
            <canvas id="overlay"></canvas>
            <button onClick={capture}><Camera size={36}/></button>
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
        <Spacer y={2}/>
        <Text h3>NOTE: Ensure your face is not covered or blocked and you are in a well lit area.</Text>
        </>
    );
}

export default FacialVerification