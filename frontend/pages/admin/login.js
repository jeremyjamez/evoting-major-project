import { Button, Card, Grid, Input, Page, Spacer, Text } from "@geist-ui/react"
import { useForm } from "react-hook-form"
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from "yup";
import { csrfToken } from 'next-auth/client'

const schema = yup.object().shape({
    username: yup.string().required(),
    password: yup.string().matches(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$@#!%*?&])[A-Za-z\d$@#!%*?&].{8,}/,
        {
            message: 'Invalid Password'
        }).required(),
});

const Login = ({csrfToken}) => {

    console.log("token: " + csrfToken)
    const { handleSubmit, register, errors } = useForm({
        resolver: yupResolver(schema)
    })

    const onSubmit = (data) => {

    }

    return (
        <>
            <Page>
                <Grid.Container justify="center">
                    <Grid xs={24} xl={12}>
                        <Card hoverable>
                            <h4>eVoting Prototype - Admin Login</h4>
                            <form method="post" action="/api/auth/callback/credentials">
                                <input name='csrfToken' type='hidden' defaultValue={csrfToken}/>
                                <Grid.Container gap={2} justify="center">
                                    <Grid xs={24}>
                                        <Input name="username" width="100%" size="large" ref={register}>Username</Input>
                                        <p>{errors.username ? errors.message.username : ''}</p>
                                    </Grid>
                                    <Grid xs={24}>
                                        <Input name="password" width="100%" type="password" size="large" ref={register}>Password</Input>
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
                                        <Button htmlType="submit" type="secondary">Login</Button>
                                    </Grid>
                                </Grid.Container>
                            </form>
                        </Card>
                        <Spacer y={.5} />
                        <Text blockquote style={{ backgroundColor: '#232323', color: 'white', border: 'none' }}>
                            <strong>Username:</strong> admin <br />
                            <strong>Password:</strong> password
                        </Text>
                    </Grid>
                </Grid.Container>
            </Page>
        </>
    )
}

Login.getInitialProps = async (context) => {
    return {
        csrfToken: await csrfToken(context)
    }
}

export default Login