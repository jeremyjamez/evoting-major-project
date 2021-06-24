import { Button, Collapse, Grid, Input, Select, Spacer, Tabs, Text, Textarea, useCurrentState, useInput, useToasts } from "@geist-ui/react"
import { useEffect, useMemo, useState } from "react";
import DataTable from "react-data-table-component"
import DashboardLayout from "./layout"
import https from 'https'
import { parseCookies } from 'nookies'
import { RefreshCw } from "@geist-ui/react-icons";
import jwt from 'jsonwebtoken'
import moment from 'moment'
import { useRoles, useUsers } from "../../utils/swr-utils";
import { useForm } from "react-hook-form";

const Users = ({token}) => {

    const columns = useMemo(() => [
        {
            name: 'UserName',
            selector: 'userName'
        },
        {
            name: 'First Name',
            selector: 'firstName'
        },
        {
            name: 'Last Name',
            selector: 'lastName'
        },
        {
            name: 'Role',
            selector: 'role'
        },
        {
            name: 'Email',
            selector: 'email'
        },
        {
            name: 'Last logged In',
            selector: row => row.lastLoggedIn ? moment(row.lastLoggedIn).format('DD/MM/YYYY hh:mm a') : ''
        }

    ], []);

    const { users } = useUsers(token)
    const { roles, isLoading } = useRoles(token)

    const [, setToast] = useToasts()

    const {state, setState, bindings} = useInput('')
    const [password, setPassword] = useState('')
    const [selectedRole, setSelectedRole, selectedRoleRef] = useCurrentState()
    const [formErrors, setFormErrors] = useState([])

    const LeftPadWithZeros = (number, length) =>{
        var str = '' + number
        while (str.length < length){
            str = '0' + str
        }
        return str
    }

    const generateId = () => {
        setState(selectedRoleRef.current + '' + LeftPadWithZeros((Math.floor(Math.random() * (9999 - 1))+1), 4))
    }

    const roleChange = (e) => {
        setSelectedRole(pre => pre = e.substr(0, 3).toUpperCase())
        generateId()
        setValue('role', e)
    }

    const generatePassword = () => {
        let password = ''
        let characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*'
        let characterLength = characters.length
        for(let i = 0; i < 10; i++){
            password += characters.charAt(Math.floor(Math.random() * characterLength))
        }
        setPassword(password)
    }

    const { register, setValue, handleSubmit } = useForm()

    useEffect(() => {
        register('role')
    },[register])

    const onSubmit = async (data) => {
        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });

        fetch(`${process.env.NEXT_PUBLIC_API_URL}/users/register`,
        { 
            agent: httpsAgent,
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            },
            body: JSON.stringify({
                role: data.role,
                userName: data.userName,
                firstName: data.firstName,
                lastName: data.lastName,
                phoneNumber: data.phoneNumber,
                trn: data.trn,
                email: data.email,
                address: data.address,
                password: data.password
            })
        })
        .then(res => {
            if(res.status === 200){
                setToast({
                    text: 'User created successfully!',
                    type: 'success'
                })
            } else {
                console.log(res.json())
                setToast({
                    text: 'Failed to create user!',
                    type: 'error'
                })
            }
        })
        .catch(error => {
            console.log(error)
            setToast({
                text: 'Failed to create user!',
                type: 'error'
            })
        })
    }

    return (
        <DashboardLayout>
            <Tabs initialValue="1" style={{margin: '16px'}}>
                <Tabs.Item value="1" label="all">
                    <Grid.Container gap={2}>
                        <Grid style={{display:'block'}} xl>
                        <DataTable
                                data={users}
                                columns={columns}
                                noHeader />
                        </Grid>
                        <Grid xl={6}>
                            <form onSubmit={handleSubmit(onSubmit)}>
                                <Grid.Container gap={2}>
                                    <Grid xl={12} style={{display: 'block'}}>
                                        Role
                                        <Spacer y={.5}/>
                                        <Select size="large" width="100%" onChange={roleChange}>
                                            {
                                                !isLoading && roles.map(role => {
                                                    return (<Select.Option key={role.id} value={role.name}><Text style={{margin: '0'}} h5>{role.name}</Text></Select.Option>)
                                                })
                                            }
                                        </Select>
                                    </Grid>
                                    <Grid xl={12}>
                                        <Input name="userName" required ref={register} size="large" width="100%" iconRight={<RefreshCw/>} iconClickable onIconClick={generateId} {...bindings}>UserName</Input>
                                    </Grid>
                                    <Grid xl={12}>
                                        <Input name="firstName" required ref={register} size="large" width="100%">First name</Input>
                                    </Grid>
                                    <Grid xl={12}>
                                        <Input name="lastName" required ref={register} size="large" width="100%">Last name</Input>
                                    </Grid>
                                    <Grid xl={12}>
                                        <Input name="phoneNumber" required ref={register} size="large" width="100%" maxLength="10">Phone number</Input>
                                    </Grid>
                                    <Grid xl={12}>
                                        <Input name="trn" required ref={register} size="large" width="100%" maxLength="9">TRN</Input>
                                    </Grid>
                                    <Grid xl={24}>
                                        <Input name="email" required type="email" ref={register} size="large" width="100%">Email</Input>
                                    </Grid>
                                    <Grid xl={24} style={{display: 'block'}}>
                                        Address
                                        <Spacer y={.5}/>
                                        <Textarea required name="address" ref={register} width="100%"/>
                                    </Grid>
                                    <Grid xl={24}>
                                        <Input name="password" required ref={register} value={password} size="large" width="100%" iconRight={<RefreshCw/>} iconClickable onIconClick={generatePassword}>Password</Input>
                                    </Grid>
                                </Grid.Container>
                                <Spacer y={1}/>
                                <Grid.Container>
                                    <Grid xl={24}>
                                        <Button htmlType="submit" style={{width: '100%'}} size="large" type="secondary" shadow>Add</Button>
                                    </Grid>
                                </Grid.Container>
                                {
                                    formErrors.length > 0 && 
                                    <Collapse shadow title="Errors" initialVisible>
                                        {
                                            formErrors.map(error => {
                                                return (<Text h4>{error}</Text>)
                                            })
                                        }
                                    </Collapse>
                                }
                            </form>
                        </Grid>
                    </Grid.Container>
                </Tabs.Item>
            </Tabs>
        </DashboardLayout>
    )
}

export async function getServerSideProps(context){
    const cookies = parseCookies(context)

    const token = cookies.token
    const decodedToken = jwt.decode(token, { complete: true })
    var dateNow = moment(moment().valueOf()).unix()

    if (decodedToken !== null && decodedToken.payload.exp < dateNow) {
        return {
            redirect: {
                destination: '/admin/login',
                permanent: false
            }
        }
    }

    return {
        props: {
            token
        }
    }
}

export default Users