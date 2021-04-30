import { Page, Button, Card, Grid, Image, Input, Note, Row, Text } from '@geist-ui/react'
import { useForm } from 'react-hook-form'
import { useRouter } from 'next/router'
import moment from 'moment'
import https from "https"
import { useState } from 'react'

export default function Home() {

  const router = useRouter()

  const { register, handleSubmit, formState: { errors } } = useForm()
  const [registered, setRegistered] = useState(false)

  const onSubmit = (data) => {
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
        if (res.ok) {

          res.json()
            .then(t => {
              if (!t.isRegistered) {
                console.log("not registered")
                setRegistered(false)
              } else {
                setRegistered(true)
                if (!t.isTwoFactorEnabled) {
                  router.push({
                    pathname: '/pair',
                    query: { voterId: data.voterId }
                  })
                } else {
                  router.push({
                    pathname: '/validatePin'
                  })
                }
              }
            })
        }
      })
      .catch(error => {
        console.log(error)
      })
  }

  return <>
    <Page>
      <Grid.Container justify="center" gap={2}>
        <Grid xs={24} xl={12}>
          <Card shadow>
            <Row justify="center" style={{ marginBottom: '15px' }}>
              <Image height={100} src="/images/electorial-logo.png" />
            </Row>

            <Row justify="center">
              <Text h3 size="1.75rem" style={{ textAlign: 'center' }}>Online eVoting Prototype</Text>
            </Row>

            <form onSubmit={handleSubmit(onSubmit)}>
              <Grid.Container gap={1} justify="center">
                <Grid xs={24}>
                  <Input width="100%" size="large" clearable ref={register({ required: true })} name="voterId">Voter ID</Input>
                </Grid>
                <Grid xs={24}>
                  {errors.voterId && <Text type="error" span>This field is required.</Text>}
                </Grid>
                <Grid xs={24}>
                  <Input width="100%" size="large" type="date" ref={register({ required: true })} name="dob">Date of Birth</Input>
                </Grid>
                <Grid xs={24}>
                  {errors.dob && <Text type="error" span>This field is required.</Text>}
                </Grid>
                <Grid>
                  <Button htmlType="submit" type="secondary" size="large">Next</Button>
                </Grid>
              </Grid.Container>
            </form>
          </Card>
        </Grid>
        <Grid xs={24}>
          <Note filled type="error" style={{ fontSize: '1.15rem', width: '100%' }}>No functionality. Click Next to move to next page.</Note>
        </Grid>
        {/* <Grid xs={24}>
                    <Note filled style={{fontSize: '1.15rem', width: '100%'}}>Enter the <strong>Elector Registration No.</strong> that is located on your <strong>Voter's ID</strong> and click Login to continue.</Note>
                </Grid> */}
      </Grid.Container>
    </Page>
  </>
}
