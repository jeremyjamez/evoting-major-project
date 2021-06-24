import { Button, Grid, Text, Modal, useModal, useCurrentState } from "@geist-ui/react"
import CandidateCard from "../components/CandidateCardComponent"
import Layout from "../components/layout"
import { parseCookies, setCookie } from 'nookies'
import jwt from 'jsonwebtoken'
import moment from 'moment'
import https from 'https'
import crypto, { privateDecrypt } from 'crypto'
import { useRouter } from "next/router"
import { useForm } from "react-hook-form"
import NodeRSA from 'node-rsa'
import { useCallback } from "react"



const SelectCandidate = ({ exp, candidates, token }) => {
    const router = useRouter()
    const { register, handleSubmit } = useForm({
        criteriaMode: 'all',
        shouldFocusError: true
    })

    const { visible, setVisible, bindings } = useModal(false)

    const [,setState, stateRef] = useCurrentState()

    const cookies = parseCookies(null)

    const onSubmit = (data) => {
        if (data.candidate) {
            setState(prev => prev = data.candidate)
            setVisible(true)
        }
    }

    const onYesClick = useCallback(() => {
        const payload = {
            voterId: cookies.voterId,
            candidateId: candidates[stateRef.current].candidateId,
            electionId: cookies.electionId,
            constituencyId: candidates[stateRef.current].constituencyId,
            ballotTime: moment().valueOf().toString()
        }
        var key = new NodeRSA(public_key)
        const encryptedPayload = key.encrypt(payload, 'base64')

        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });

        fetch(`${process.env.NEXT_PUBLIC_API_URL}/votes`,
            {
                agent: httpsAgent,
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + token
                },
                body: JSON.stringify(encryptedPayload)
            })
            .then(res => {
                if (res.ok) {
                    res.json()
                        .then(voteId => {
                            if (!voteId) {
                                setToast({
                                    text: 'Failed to cast vote. Please wait and try again.',
                                    type: 'error'
                                })
                            } else {
                                setToast({
                                    text: 'Vote successfully cast.',
                                    type: 'success'
                                })
                                setCookie(null, 'voteId', voteId)
                                router.push('/vote-result')
                            }
                        })
                }
            })
            .catch(error => {
                setToast({
                    text: 'Failed to cast vote. Please wait and try again.',
                    type: 'error'
                })
                console.log(error)
            })
            setVisible(false)
    }, [])

    return (
        <Layout expireTimestamp={exp}>
            <Grid.Container gap={2}>
                <Grid xs={24}>
                    <Text h2>Below are the {candidates.length} candidates for {candidates[0].constituencyName}. Choose ONLY ONE.</Text>
                </Grid>

                <Grid xs={24} style={{ display: 'block' }}>
                    <form onSubmit={handleSubmit(onSubmit)}>
                        {
                            candidates.map((candidate, idx) => {
                                return <label className="radio-container" key={candidate.candidateId}>
                                    <CandidateCard candidate={candidate} token={token} />
                                    <input type="radio" name="candidate" ref={register} value={idx}></input>
                                    <span className="x"></span>
                                </label>
                            })
                        }
                        <Grid.Container justify="flex-end">
                            <Grid>
                                <Button htmlType="submit" type="secondary" size="large" shadow>
                                    <Text h3>Vote</Text>
                                </Button>
                            </Grid>
                        </Grid.Container>
                    </form>
                </Grid>
            </Grid.Container>

            <Modal {...bindings} disableBackdropClick={true}>
                <Modal.Title>
                    <Text h2>Confirm Vote?</Text>
                </Modal.Title>
                <Modal.Content>
                    <Text h3>Ensure you have selected the candidate you wish to vote for. You can click No to double check your selection OR Yes to confirm your vote.</Text>
                    <Text type="error" h3>IMPORTANT: You will NOT be able to change your selection after clicking Yes.</Text>
                </Modal.Content>
                <Modal.Action passive onClick={() => setVisible(false)} style={{ fontSize: '1.45rem', fontWeight: '800' }}>No</Modal.Action>
                <Modal.Action style={{ fontSize: '1.45rem', fontWeight: '800' }} onClick={onYesClick}>Yes</Modal.Action>
            </Modal>
            <style jsx>{`
                .radio-container {
                    display: block;
                    position: relative;
                    width: 100%;
                    cursor: pointer;
                    -webkit-user-select: none;
                    -moz-user-select: none;
                    -ms-user-select: none;
                    user-select: none;
                    margin-bottom: 12px;
                }

                .radio-container input {
                    position: absolute;
                    opacity: 0;
                    cursor: pointer;
                }

                .x {
                    position: absolute;
                    top: 0;
                    right: 0;
                    height: 50px;
                    width: 50px;
                    border: 2px solid black;
                }

                /* On mouse-over, add a grey background color */
                .radio-container:hover input ~ .x {
                    background-color: #ccc;
                }

                /* Create the indicator (the X - hidden when not checked) */
                .x:after {
                    content: "X";
                    position: absolute;
                    display: none;
                }

                /* Show the indicator (X) when checked */
                .radio-container input:checked ~ .x:after {
                    display: block;
                }

                /* Style the indicator (dot/circle) */
                .radio-container .x:after {
                    font-size: 2.5rem;
                    top: 0;
                    left: 10px;
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

        const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/candidates/${tokenData.Id}`,
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

            /* const decryptedData = privateDecrypt({
                key: cookies.private_key,
                padding: crypto.constants.RSA_PKCS1_OAEP_PADDING,
                oaepHash: 'sha1',
                passphrase: `${process.env.NEXT_PUBLIC_PRIVATE_KEY_PASS}`
            }, Buffer.from(data, 'base64')) */

            const candidates = data

            return {
                props: {
                    candidates,
                    token,
                    exp: tokenData.exp
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

export default SelectCandidate