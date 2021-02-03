import { Avatar, Button, Card, Grid, Image, Page } from "@geist-ui/react"
import CandidateCard from "../components/CandidateCardComponent"
import Link from "next/link"

const SelectCandidate = () => {
    return (
        <>
            <Page>
                <Grid.Container gap={2}>
                    <Grid xs={24}>
                        <h1>Below are the candidates within your constituency</h1>
                    </Grid>

                    <Grid xs={24}>
                        <CandidateCard />
                        {/* <Card hoverable>
                            <Grid.Container>
                                <Grid xs={2}>
                                    <div className="round">
                                        <input type="checkbox" id="checkbox"/>
                                        <label for="checkbox"/>
                                    </div>
                                    <style jsx>{`
                                        .round {
                                            position: relative;
                                        }

                                        .round label {
                                            background-color: #fff;
                                            border: 1px solid #ccc;
                                            border-radius: 50%;
                                            cursor: pointer;
                                            height: 28px;
                                            left: 0;
                                            position: absolute;
                                            top: 0;
                                            width: 28px;
                                        }

                                        .round label:after {
                                            border: 2px solid #fff;
                                            border-top: none;
                                            border-right: none;
                                            content: "";
                                            height: 6px;
                                            left: 7px;
                                            opacity: 0;
                                            position: absolute;
                                            top: 8px;
                                            transform: rotate(-45deg);
                                            width: 12px;
                                        }

                                        .round input[type="checkbox"] {
                                            visibility: hidden;
                                        }
                                          
                                        .round input[type="checkbox"]:checked + label {
                                            background-color: #66bb6a;
                                            border-color: #66bb6a;
                                        }
                                          
                                        .round input[type="checkbox"]:checked + label:after {
                                            opacity: 1;
                                        }
                                    `}</style>
                                </Grid>
                            </Grid.Container>
                        </Card> */}
                    </Grid>
                </Grid.Container>
                <Grid.Container justify="flex-end">
                    <Grid>
                        <Link href="confirmation"><Button type="secondary" size="large" auto shadow>Next</Button></Link>
                    </Grid>
                </Grid.Container>
            </Page>
        </>
    )
}

export default SelectCandidate