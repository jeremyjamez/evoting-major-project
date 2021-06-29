import { Button, Dot, Grid, Input, Popover, Spacer, useToasts, Note, Avatar, Progress, Image } from "@geist-ui/react"
import { Trash2, X } from "@geist-ui/react-icons"
import { useCallback, useEffect, useMemo, useState } from "react"
import DataTable from "react-data-table-component"
import { useMembers, usePoliticalParties } from "../../utils/swr-utils"
import DashboardLayout from "./layout"
import { HexColorInput, HexColorPicker } from "react-colorful";
import "react-colorful/dist/index.css";
import moment from "moment"
import { useForm } from "react-hook-form"
import { useDropzone } from "react-dropzone"
import axios from "axios"
import https from "https"
import jwt from 'jsonwebtoken'
import { parseCookies } from 'nookies'

const baseStyle = {
    flex: 1,
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    padding: '20px',
    borderWidth: 2,
    borderRadius: 2,
    borderColor: '#eeeeee',
    borderStyle: 'dashed',
    backgroundColor: '#fafafa',
    color: '#bdbdbd',
    outline: 'none',
    transition: 'border .24s ease-in-out'
};

const activeStyle = {
    borderColor: '#2196f3'
};

const acceptStyle = {
    borderColor: '#00e676'
};

const rejectStyle = {
    borderColor: '#ff1744'
};

const PoliticalParties = ({ token }) => {
    const { parties } = usePoliticalParties(token)
    const { members } = useMembers(token)

    const [file, setFile] = useState()
    const [progress, setProgress] = useState(0)
    const [colour, setColour] = useState("#000000")

    const columns = useMemo(() => [
        {
            name: 'Party Id',
            selector: 'partyId'
        },
        {
            name: 'Name',
            selector: 'longName'
        },
        {
            name: 'Icon',
            center: true,
            cell: row => row.icon !== "" ? <Image width={30} height={30} src={row.icon} /> : ""
        },
        {
            name: 'Colour',
            cell: row => <span style={{ backgroundColor: row.colour, borderRadius: '50%', height: '40px', width: '40px' }}></span>
        }
    ])

    const [, setToast] = useToasts();


    const onDrop = useCallback(acceptedFiles => {
        const fileData = new FormData()
        fileData.append('file', acceptedFiles[0], acceptedFiles[0].name)
        axios.post(`${process.env.apiUrl}/upload`, fileData, {
            onUploadProgress: (e) => {
                setProgress(Math.round(100 * e.loaded / e.total))
            },
        })
            .then(res => {
                if (res.data.statusCode === 200) {
                    setToast({
                        text: 'Upload Complete',
                        type: 'success'
                    })
                    setFile('https://localhost:44387/' + res.data.dbPath)
                    setValue("partyIcon", 'https://localhost:44387/' + res.data.dbPath)
                } else {
                    setToast({
                        text: 'Upload Failed',
                        type: 'error'
                    })
                }
                setProgress(0)
            })
    }, [])

    const { getRootProps,
        getInputProps,
        isDragActive,
        isDragAccept,
        isDragReject } = useDropzone({ onDrop, accept: 'image/jpeg, image/png', maxFiles: 1 })

    const style = useMemo(() => ({
        ...baseStyle,
        ...(isDragActive ? activeStyle : {}),
        ...(isDragAccept ? acceptStyle : {}),
        ...(isDragReject ? rejectStyle : {})
    }), [
        isDragActive,
        isDragReject,
        isDragAccept
    ]);

    const { register, setValue, handleSubmit } = useForm()
    const onSubmit = async (data) => {

        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });

        fetch(`${process.env.apiUrl}/politicalparties`, {
            agent: httpsAgent,
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                'name': data.partyName,
                'icon': data.partyIcon === undefined ? "none" : data.partyIcon,
                'colour': data.partyColour,
                'founded': moment(data.foundedDate).toISOString()
            })
        })
            .then(res => {
                if (res.status === 201) {
                    setToast({
                        text: 'Successfully added!',
                        type: 'success'
                    })
                } else {
                    setToast({
                        text: 'Failed to add!',
                        type: 'error'
                    })
                }
            })
            .catch(error => {
                console.log(error)
                setToast({
                    text: 'Failed to add!',
                    type: 'error'
                })
            })
    }

    const handleColourChange = (e) => {
        setColour(e)
        setValue("partyColour", e)
    }

    useEffect(() => {
        register("partyColour")
        register("partyIcon")
    }, [register, register])

    const handleDelete = async (id) => {
        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        });
        fetch(`${process.env.apiUrl}/politicalparties/${id}`, {
            agent: httpsAgent,
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        })
            .then(res => res.status)
            .then(status => {
                if (status === 204) {
                    setToast({
                        text: 'Record removed successful!',
                        type: 'success'
                    });
                }
            })
            .catch(error => {
                console.log(error)
                setToast({
                    text: 'Failed to remove record!',
                    type: 'error'
                });
            })
    }

    return (
        <DashboardLayout>
            <div style={{ margin: '16px' }}>
                <Grid.Container gap={2}>
                    <Grid xl={18} style={{ display: 'block' }}>
                        <DataTable
                            columns={columns}
                            data={parties}
                            noHeader
                        />
                    </Grid>
                    <Grid xl={6}>
                        <form onSubmit={handleSubmit(onSubmit)}>
                            <Grid.Container justify="center" gap={2} alignItems="flex-end">
                                <Grid xl={24}>
                                    <Input width="100%" name="partyName" ref={register({ required: true })} size="large">Name</Input>
                                </Grid>
                                <Grid xl={24} style={{ display: 'block' }}>
                                    Icon
                                <div {...getRootProps({ style })}>
                                        <input {...getInputProps()} />
                                        <p>Drag and drop icon file here, or click to select file</p>
                                    </div>
                                    {
                                        progress > 0 ?
                                            <>
                                                <Spacer y={.5} />
                                                <Progress value={progress} hidden={progress === 100} />
                                            </>
                                            : null
                                    }
                                    {
                                        file ?
                                            <>
                                                <Spacer y={.5} />
                                                <Note label="file">{file}</Note>
                                            </> : null
                                    }

                                </Grid>
                                <Grid xl={24} style={{ display: 'block' }}>
                                    Party Colour
                                <Spacer y={.5} />
                                    <Popover trigger="hover" content={<HexColorPicker color={colour} onChange={handleColourChange} />}>
                                        <Dot><HexColorInput placeholder="Party Colour" color={colour} onChange={handleColourChange} /></Dot>
                                    </Popover>
                                </Grid>
                                <Grid>
                                    <Button id="addBtn" htmlType="submit" type="secondary" style={{ width: '100%' }} shadow>Add</Button>
                                </Grid>
                            </Grid.Container>
                        </form>
                    </Grid>
                </Grid.Container>
                <style global jsx>{`
                .tooltip-content.popover > .inner {
                    padding: 0 !important
                }
                .dot > .icon {
                    width: 30px;
                    height: 30px;
                    background-color: ${colour}
                }
                #addBtn.btn:hover, #addBtn.btn:focus {
                    background-color: ${colour} !important;
                    border-color: ${colour} !important
                }
            `}</style>
            </div>
        </DashboardLayout>
    )
}

export async function getServerSideProps(context) {
    const cookies = parseCookies(context)

    const token = cookies.to
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

export default PoliticalParties