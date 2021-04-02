import { Button, Card, Grid, Input, Page, Spacer, Text, useToasts } from "@geist-ui/react"
import { useForm } from "react-hook-form"
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from "yup";
import https from 'https'
import { useRouter } from 'next/router'
import { useState } from "react";
import jwt from 'jsonwebtoken'
import moment from 'moment'
import nookies, { parseCookies, setCookie, destroyCookie } from 'nookies'

const schema = yup.object().shape({
    username: yup.string().required(),
    password: yup.string().matches(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$@#!%*?&])[A-Za-z\d$@#!%*?&].{8,}/,
        {
            message: 'Invalid Password'
        }).required(),
});

const Login = () => {

    const { handleSubmit, register, errors } = useForm({
        resolver: yupResolver(schema)
    })

    const router = useRouter()
    const [isLoading, setLoading] = useState(false)

    const [, setToast] = useToasts()

    const onSubmit = async (data) => {

        setLoading(true)

        /* const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        }); */

        fetch(`${process.env.NEXT_PUBLIC_API_URL}/authmanagement/login`,
            {
                //agent: httpsAgent,
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    "username": data.username,
                    "password": data.password
                })
            })
            .then(res => {
                return res.json()
            })
            .then(token => {
                if (token.errors !== null && token.errors.length > 0) {
                    setLoading(false)
                    setToast({
                        text: 'Failed to login',
                        type: 'error'
                    })
                } else {
                    setToast({
                        text: 'Sign in successful',
                        type: 'success'
                    })

                    setCookie(null, 'token', token.token, {
                        maxAge: 60 * 60 * 1000,
                        path: '/'
                    })

                    router.push('/admin/home')
                }
            })
    }

    return (
        <>
            <Page>
                <Grid.Container justify="center" gap={2}>
                    <Grid xs={24} xl={12}>
                        <Card hoverable>
                            <h4>eVoting Prototype - Admin Login</h4>
                            <form onSubmit={handleSubmit(onSubmit)}>
                                <Grid.Container gap={2} justify="center">
                                    <Grid xs={24}>
                                        <Input name="username" width="100%" size="large" ref={register} disabled={isLoading}>Username</Input>
                                        <p>{errors.username ? errors.message.username : ''}</p>
                                    </Grid>
                                    <Grid xs={24}>
                                        <Input.Password name="password" width="100%" size="large" ref={register} disabled={isLoading}>Password</Input.Password>
                                        {
                                            errors.password ?
                                                <Text type="error">
                                                    Password should contain at least 8 characters <br />
                                                    Should contain at least 1 uppercase letter [A-Z] <br />
                                                    Should contain at least 1 lowercase letter [a-z] <br />
                                                    Should contain at least 1 special character [$@#!%*?&]
                                                </Text> : ''
                                        }
                                    </Grid>
                                    <Grid xs={12}>
                                        <Button htmlType="submit" type="secondary" shadow loading={isLoading}>Login</Button>
                                    </Grid>
                                </Grid.Container>
                            </form>
                        </Card>
                    </Grid>
                    <Grid xl={24}>
                        <Text blockquote style={{ backgroundColor: '#232323', color: 'white', border: 'none', margin: '0 auto' }}>
                            <strong>Username:</strong> john <br />
                            <strong>Password:</strong> Password@123
                        </Text>
                    </Grid>
                </Grid.Container>
            </Page>
        </>
    )
}

export async function getServerSideProps(ctx) {
    const cookies = nookies.get(ctx)

    const token = cookies.token
    const decodedToken = jwt.decode(token, { complete: true })
    var dateNow = moment(moment().valueOf()).unix()

    if (decodedToken !== null && decodedToken.payload.exp > dateNow) {
        return {
            redirect: {
                destination: '/admin/home',
                permanent: false
            }
        }
    }

    destroyCookie(ctx, 'token', {path: '/'})

    return {
        props: {}
    }
}

export default Login