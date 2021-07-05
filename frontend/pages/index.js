import { Button, Card, Grid, Image, Input, Page, Row, Text, Modal, useModal } from '@geist-ui/react'
import { useRouter } from 'next/router'
import { useState } from 'react'
import { useForm } from 'react-hook-form'
import moment from 'moment'
import https from "https"
import styles from '../styles/layout.module.css'
import { yupResolver } from '@hookform/resolvers/yup'
import * as yup from 'yup'
import nookies, { destroyCookie, setCookie } from 'nookies'
import { generateKeyPairSync } from 'crypto'

const schema = yup.object().shape({
  voterId: yup.string().required('Voter ID is a required field.').length(7, 'Voter ID must be exactly 7 numbers.'),
  dob: yup.string().required('Date of Birth is a required field.')
})

export default function Home({ publicKey, csrfToken }) {

  const router = useRouter()

  const { register, handleSubmit, formState: { errors } } = useForm({
    resolver: yupResolver(schema)
  })

  const [registered, setRegistered] = useState(true)
  const [loading, setLoading] = useState(false)
  const [fetchError, setFetchError] = useState('')
  const { visible, setVisible, bindings } = useModal(false)

  const onSubmit = (data) => {
    setLoading(true)

    const payload = {
      voterId: data.voterId,
      DateofBirth: moment(data.dob).toISOString(),
      publicKey: publicKey,
      currentTime: Math.trunc(moment().valueOf() / 1000).toString()
    }

    const httpsAgent = new https.Agent({
      rejectUnauthorized: false,
    });

    fetch(`${process.env.NEXT_PUBLIC_API_URL}/voters/isregistered`,
      {
        agent: httpsAgent,
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          'XSRF-TOKEN': csrfToken
        },
        body: JSON.stringify(payload)
      })
      .then(res => {
        if (!res.ok) {
          throw new Error("Network error. Unable to connect to server.")
        }
        return res.json()
      })
      .then(t => {
        if (!t.isRegistered) {
          setRegistered(false)
          setLoading(false)
        } else {
          setRegistered(true)
          setLoading(false)

          if (t.hasVoted) {
            setVisible(true)
          } else {
            setCookie(null, 'public_key', t.publicKey)
            setCookie(null, 'voterId', data.voterId)
            setCookie(null, 'electionId', t.electionId)

            if (!t.isTwoFactorEnabled) {
              router.push('/pair')
            } else {
              router.push('/validatePin')
            }
          }
        }
      })
      .catch((error) => {
        setFetchError(`There has been a problem completing the request. ${error}`)
        setRegistered(false)
        setLoading(false)
      })
  }

  return (
    <Page size="large">
      <Grid.Container gap={4}>
        <Grid xl={14} style={{ display: 'block' }}>
          <Text h2>Watch the video below on how to cast your vote before starting.</Text>
          <iframe width="100%" height="315" src="https://www.youtube.com/embed/1a0wxn-k-lk" title="YouTube video player" 
          frameBorder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowFullScreen></iframe>
        </Grid>
        <Grid xl={10}>
          <Grid.Container justify="center" gap={2}>
            <Grid xs={24}>
              <Card shadow>
                <Row justify="center" style={{ marginBottom: '15px' }}>
                  <Image height={100} src="/images/electorial-logo.png" />
                </Row>

                <Row justify="center">
                  <Text h3 size="1.75rem" style={{ textAlign: 'center' }}>Online E-Voting Prototype</Text>
                </Row>

                <form onSubmit={handleSubmit(onSubmit)}>
                  <Grid.Container gap={1} justify="center">
                    <Grid xs={24}>
                      <Input width="100%" size="large" clearable ref={register({ required: true, maxLength: 7, minLength: 7 })} name="voterId">
                        <Text h3>Voter ID</Text>
                      </Input>
                    </Grid>
                    <Grid xs={24}>
                      {
                        errors.voterId && <Text className={styles.formError} span>{errors.voterId?.message}</Text>
                      }
                    </Grid>
                    <Grid xs={24}>
                      <Input width="100%" size="large" type="date" ref={register({ required: true })} name="dob">
                        <Text h3>Date of Birth</Text>
                      </Input>
                    </Grid>
                    <Grid xs={24}>
                      {errors.dob && <Text className={styles.formError} span>{errors.dob?.message}</Text>}
                    </Grid>
                    <Grid>
                      <Button htmlType="submit" type="secondary" size="large" loading={loading}>
                        <Text h3>Next</Text>
                      </Button>
                    </Grid>
                    <Grid xs={24}>
                      {
                        !registered ? <Text className={styles.formError} span>Voter not found.</Text> : null
                      }
                    </Grid>
                  </Grid.Container>
                </form>
              </Card>
            </Grid>
            <Grid>
              {
                fetchError !== '' ? <Text className={styles.formError} span>{fetchError}</Text> : null
              }
            </Grid>
          </Grid.Container>
        </Grid>
      </Grid.Container>
      <style jsx>{`
        ul > li {
          font-size: 1.4rem;
        }
      `}</style>
      <Modal {...bindings} disableBackdropClick={true}>
        <Modal.Content>
          <Text h3>You have already voted in this election.</Text>
        </Modal.Content>
        <Modal.Action onClick={() => {
          destroyCookie(null, 'private_key')
          router.push('/')
          setVisible(false)
        }}>OK</Modal.Action>
      </Modal>
    </Page>
  )
}

export async function getServerSideProps(context) {

  const { publicKey, privateKey } = generateKeyPairSync('rsa', {
    modulusLength: 4096,
    publicKeyEncoding: {
      type: 'pkcs1',
      format: 'pem'
    },
    privateKeyEncoding: {
      type: 'pkcs1',
      format: 'pem',
      cipher: 'aes-256-cbc',
      passphrase: `${process.env.NEXT_PUBLIC_PRIVATE_KEY_PASS}`
    }
  })

  destroyCookie(context, 'private_key')
  destroyCookie(context, 'token')
  destroyCookie(context, 'electionId')
  destroyCookie(context, 'public_key')
  destroyCookie(context, 'data')
  destroyCookie(context, 'voterId')

  nookies.set(context, 'private_key', privateKey)

  return {
    props: {
      publicKey
    }
  }


}
