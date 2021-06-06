import { Grid, Text, Tooltip, Link, Button, useModal, Modal, Select, Spacer, Input, useToasts, useCurrentState } from "@geist-ui/react"
import menuItems from './components/navMenu'
import { default as NextLink } from 'next/link'
import { withRouter } from "next/router"
import { Plus } from "@geist-ui/react-icons"
import { useEffect, useState } from "react"
import { useForm } from "react-hook-form"
import { yupResolver } from '@hookform/resolvers/yup'
import * as yup from "yup"
import moment from "moment"
import https from "https"
import { parseCookies } from "nookies"

const schema = yup.object().shape({

})

const DashboardLayout = (props) => {

    const [, setElectionDate] = useState();
    const [, setElectionStartTime, startTimeRef] = useCurrentState()
    const [, setElectionEndTime, endTimeRef] = useCurrentState()
    const { setVisible, bindings } = useModal()

    const token = parseCookies(null, 'token').token

    const [, setToast] = useToasts()

    const { handleSubmit, register, errors, setValue, setError, clearErrors } = useForm({
        resolver: yupResolver(schema)
    })

    useEffect(() => {
        register('electionType', {required: true})
        register('time')
    },[register, register])

    const selectHandler = (e) => {
        setValue('electionType', e)
    }

    const checkIfTimeEqual = (start, end) => {
        if(start === end){
            setError('time', { type: 'manual', message: 'Start time and End time cannot be the same' })
        } else {
            clearErrors('time')
        }
    }

    const electionStartTimeHandler = (e) => {
        setElectionStartTime((prev) => prev = e.target.value)
        checkIfTimeEqual(startTimeRef.current, endTimeRef.current)
    }

    const electionEndTimeHandler = (e) => {
        setElectionEndTime((prev) => prev = e.target.value)
        checkIfTimeEqual(startTimeRef.current, endTimeRef.current)
    }

    const electionDateHandler = (e) => {
        setElectionDate(e.target.value)
    }

    const onSubmit = (data) => {
        let date = moment(data.electionDate).valueOf() / 1000
        let startTime = moment(data.electionDate + " " + data.electionStartTime).valueOf() / 1000
        let endTime = moment(data.electionDate + " " + data.electionEndTime).valueOf() / 1000

        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });
        fetch(`${process.env.NEXT_PUBLIC_API_URL}/Elections`, {
            agent: httpsAgent,
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            },
            body: JSON.stringify({
                "type": data.electionType,
                "date": date.toString(),
                "startTime": startTime.toString(),
                "endTime": endTime.toString(),
            })
        })
            .then(response => {
                if (response.status === 201) {
                    setToast({
                        text: 'Successfully saved to database!',
                        type: 'success'
                    });
                    setVisible(false)
                } else {
                    setToast({
                        text: 'Failed to save to database!',
                        type: 'error'
                    });
                }
            })
            .catch(error => console.log(error))
    }

    return (
        <>
            <Grid.Container style={{ flex: '1 1 auto' }}>
                <Grid
                    xl={1}
                    style={{
                        borderRight: "1px solid #e2e2e2",
                        backgroundColor: "#f8f8f8",
                        color: "white",
                        paddingLeft: "8px",
                        paddingRight: "8px"
                    }}>
                    <div className="sideMenu" style={{ position: 'fixed', display: 'grid' }}>
                        {menuItems.map((item) => (
                            <Tooltip
                                key={item.path}
                                text={item.label}
                                placement="right"
                                type="dark">
                                <NextLink href={item.path} key={item.path}>
                                    <Link
                                        className={`NavButton ${props.router.pathname === item.path ? "active" : ""
                                            }`}
                                    >
                                        <div>{item.icon}</div>
                                    </Link>
                                </NextLink>
                            </Tooltip>

                        ))}
                    </div>
                </Grid>
                <Grid xs={24} xl={23} style={{ display: 'flex', flexFlow: 'column', height: '100%' }}>
                    <Grid.Container>
                        <Grid xs={24}>
                            <Grid.Container style={{ margin: '16px' }} justify="space-between">
                                <Grid justify="flex-end">
                                    <Text h3>Online E-Voting Prototype - Admin</Text>
                                </Grid>
                                <Grid>
                                    <Button icon={<Plus />} onClick={() => setVisible(true)} auto shadow type="secondary" size="large">New Election</Button>
                                </Grid>
                            </Grid.Container>
                        </Grid>
                        <Grid xs={24}>
                            {props.children}
                        </Grid>
                    </Grid.Container>
                </Grid>
            </Grid.Container>
            <Modal {...bindings}>
                <Modal.Title>
                    <Text>Add new election</Text>
                </Modal.Title>
                <Modal.Content>
                    <form id="add_form" onSubmit={handleSubmit(onSubmit)}>
                        <Grid.Container gap={2}>
                            <Grid xs={24} style={{ display: 'block' }}>
                                Election Type
                                <Spacer y={.5} />
                                <Select placeholder="Election Type" width="100%" size="large" name="electionType" onChange={selectHandler}>
                                    <Select.Option value="General Election">General Election</Select.Option>
                                </Select>
                            </Grid>
                            <Grid xs={24}>
                                <Input ref={register} name="electionDate" type="date" onChange={electionDateHandler} size="large" width="100%" >Election Date</Input>
                            </Grid>
                            <Grid xs={12}>
                                <Input ref={register} name="electionStartTime" type="time" onChange={electionStartTimeHandler} size="large" width="100%" >Start</Input>
                            </Grid>
                            <Grid xs={12}>
                                <Input ref={register} name="electionEndTime" type="time" onChange={electionEndTimeHandler} size="large" width="100%" >End</Input>
                            </Grid>
                            <Grid xs={24}>
                                { errors.time ? <Text type="error">{errors.time?.message}</Text> : '' }
                            </Grid>
                        </Grid.Container>
                    </form>
                </Modal.Content>
                <Modal.Action passive onClick={() => setVisible(false)}>Cancel</Modal.Action>
                <Modal.Action htmlType="submit" form="add_form">Add</Modal.Action>
            </Modal>
            <style global jsx>{`
            body {
                background-color: white !important;
            }
            .tooltip {
                display: inherit;
            }
            .NavButton {
                color: #575151 !important;
                width: 100%;
                height: 55px;
                border: 1px solid #f0f0f0;
                padding: 16px;
                vertical-align: middle;
                border-radius: 10px !important;
                background-color: white;
            }

            .tooltip:first-child{
                margin-top: 10px;
            }

            .tooltip:not(:last-child){
                margin-bottom: 20px !important;
            }

            .NavButton.active {
                background-color: #000000 !important;
                color: white !important;
                box-shadow: 0px 10px 20px 0px #bbbbbb;
                border: none;
            }
            .NavButton.active span.icon {
                color: white !important;
            }
        `}</style>
        </>
    )
}

export default withRouter(DashboardLayout);