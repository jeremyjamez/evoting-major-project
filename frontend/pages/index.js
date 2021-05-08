import { Button, Card, Grid, Image, Input, Page, Row, Text } from '@geist-ui/react'
import { useForm } from 'react-hook-form'
import { useRouter } from 'next/router'
import moment from 'moment'
import https from "https"
import { useState } from 'react'
import styles from '../styles/layout.module.css'
import { yupResolver } from '@hookform/resolvers/yup'
import * as yup from 'yup'

const schema = yup.object().shape({
  voterId: yup.string().required('Voter ID is a required field.').length(7, 'Voter ID must be exactly 7 numbers.'),
  dob: yup.string().required('Date of Birth is a required field.')
})

export default function Home() {

  const router = useRouter()

  const { register, handleSubmit, formState: { errors } } = useForm({
    resolver: yupResolver(schema)
  })

  const [registered, setRegistered] = useState(true)
  const [loading, setLoading] = useState(false)
  const [fetchError, setFetchError] = useState('')

  const onSubmit = (data) => {
    setLoading(true)

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
        },
        body: JSON.stringify({
          voterId: data.voterId,
          DateofBirth: moment(data.dob).toISOString()
        })
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
          if (!t.isTwoFactorEnabled) {
            router.push({
              pathname: '/pair',
              query: { voterId: data.voterId }
            })
          } else {
            router.push({
              pathname: '/validatePin',
              query: { voterId: data.voterId }
            })
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
    <Page>
      <Grid.Container justify="center" gap={2}>
        <Grid xs={24} xl={12}>
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
                  <Input width="100%" size="large" clearable ref={register({ required: true, maxLength: 7, minLength: 7 })} name="voterId">Voter ID</Input>
                </Grid>
                <Grid xs={24}>
                  {
                    errors.voterId && <Text className={styles.formError} span>{errors.voterId?.message}</Text>
                  }
                </Grid>
                <Grid xs={24}>
                  <Input width="100%" size="large" type="date" ref={register({ required: true })} name="dob">Date of Birth</Input>
                </Grid>
                <Grid xs={24}>
                  {errors.dob && <Text className={styles.formError} span>{errors.dob?.message}</Text>}
                </Grid>
                <Grid>
                  <Button htmlType="submit" type="secondary" size="large" loading={loading}>Next</Button>
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
    </Page>
  )
}
