import { Grid, Loading } from "@geist-ui/react"
import { useGetParty } from "../utils/swr-utils"

const CandidateCard = ({ candidate, token }) => {

    const { party, isLoading } = useGetParty(candidate.affiliation, token)
    if (isLoading) {
        return <Loading />
    }

    return (
        <>
            <div className="candidate-container">
                <div className="candidate">
                    <Grid.Container justify="flex-start">
                        <Grid xs={10}>
                            <div className="candidate-info">
                                <h2>{candidate.lastName}, {candidate.firstName}</h2>
                                <h5>Candidate Address</h5>
                            </div>
                        </Grid>
                        <Grid xs alignItems="center">
                            <div className="candidate-image">
                                <img src={party.icon} width="36" height="32" />
                            </div>
                        </Grid>
                    </Grid.Container>
                </div>
            </div>
            <style jsx>{`
                            .candidate-container {
                                margin-bottom: 10px;
                                width: 100%;
                            }

                            .candidate:hover {
                                box-shadow: 0px 8px 10px 0px rgba(0, 0, 0, 0.05)
                            }

                            .candidate {
                                background-color: white;
                                border-radius: 10px;
                                display: flex;
                                max-width: 100%;
                                overflow: hidden;
                            }

                            .candidate h5 {
                                opacity: 0.6;
                                margin: 0;
                                letter-spacing: 1px;
                                text-transform: uppercase;
                            }

                            .candidate h2 {
                                letter-spacing: 1px;
                                margin: 10px 0 5px 0;
                            }

                            .candidate-image {
                                max-width: 100%;
                                margin: 0 auto;
                                width: 2.5rem;
                            }
                            
                            .candidate-info {
                                padding: 30px;
                                position: relative;
                                width: 100%;
                                display: block;
                            }

                            .candidate-title {
                                font-size: 1rem;
                                opacity: 0.6;
                            }
                        `}</style>
        </>
    )
}

export default CandidateCard