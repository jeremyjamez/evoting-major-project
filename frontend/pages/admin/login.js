import { Button, Card, Grid, Input, Page, Spacer, Text } from "@geist-ui/react"
import { useForm } from "react-hook-form"
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from "yup";

const schema = yup.object().shape({
    username: yup.string().required(),
    password: yup.string().matches(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$@#!%*?&])[A-Za-z\d$@#!%*?&].{8,}/,
        {
            message: {
                error1: 'Password should contain at least 8 characters',
                error2: 'Should contain at least 1 uppercase letter [A-Z]',
                error3: 'Should contain at least 1 lowercase letter [a-z]'
            }
        }).required(),
});

const Login = () => {

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
                            <form onSubmit={handleSubmit(onSubmit)}>
                                <Grid.Container gap={2} justify="center">
                                    <Grid xs={24}>
                                        <Input name="username" width="100%" size="large" ref={register}>Username</Input>
                                        <p>{errors.username?.message}</p>
                                    </Grid>
                                    <Grid xs={24}>
                                        <Input name="password" width="100%" type="password" size="large" ref={register}>Password</Input>
                                        <ul type="*">
                                            <li>{errors.password?.message.error1}</li>
                                            <li>{errors.password?.message.error2}</li>
                                            <li>{errors.password?.message.error3}</li>
                                        </ul>
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

export default Login