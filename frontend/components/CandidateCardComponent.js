import { Avatar, Grid, Loading } from "@geist-ui/react"
import { useGetParty } from "../utils/swr-utils"

const CandidateCard = ({candidate, token}) => {

    const {party, isLoading} = useGetParty(candidate.affiliation, token)

    if(isLoading){
        return <Loading/>
    }
    
    return (
        <>
            <div className="candidate-container">
                <div className="candidate">
                    <Grid.Container>
                        <Grid xs={24} md={6} lg={6} xl={7}>
                            <div className="candidate-image">
                                <div className="rounded-img">
                                    {/* <img src="https://jis.gov.jm/media/AndrewXHolnessX11x14XXOfficialXB-2.jpg" /> */}
                                </div>
                            </div>
                        </Grid>
                        <Grid xs={24} md lg xl>
                            <div className="candidate-info">
                                <div className="party-icon-container">
                                    <img src={party.icon} />
                                </div>
                                <h6>{candidate.constituencyName} - {candidate.parish}</h6>
                                <h2>{candidate.fullName}</h2>
                                {/* <span className="candidate-title">Leader of the Jamaica Labour Party</span> */}
                            </div>
                        </Grid>
                    </Grid.Container>


                </div>
                <style jsx>{`
                            .candidate-container {
                                margin-bottom: 20px;
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

                            .candidate h6 {
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
                                background-color: ${party.colour};
                                padding: 30px;
                                max-width: 100%;
                            }

                            .candidate-image .rounded-img {
                                border-radius: 50%;
                                width: 12rem;
                                height: 12rem;
                                overflow: hidden;
                                margin: 0 auto;
                            }
                            
                            .candidate-info {
                                padding: 30px;
                                position: relative;
                                width: 100%;
                            }

                            .candidate-title {
                                font-size: 1rem;
                                opacity: 0.6;
                            }

                            .party-icon-container {
                                position: absolute;
                                top: 30px;
                                right: 30px;
                            }

                            .party-icon-container img {
                                width: 2rem;
                                height: 2rem;
                            }
                        `}</style>
            </div>
        </>
    )
}

export default CandidateCard