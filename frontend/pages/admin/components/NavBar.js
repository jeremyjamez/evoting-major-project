import { Grid, Spacer, Text, Row } from "@geist-ui/react";
import NavButton from "./NavButton";
import menuItems from "./navMenu";

const NavBar = props => {
    return (
        <Grid.Container style={{ height: '100vh' }}>
            <Grid xl={1} style={{ borderRight: '1px solid #e2e2e2', backgroundColor: '#f8f8f8', color: 'white', 
            paddingLeft: '8px', paddingRight: '8px' }}>
                <Spacer y={4} />
                <Grid.Container gap={2}>
                    {menuItems.map(item => (
                        <Grid xs={24} key={item.path}>
                            <NavButton
                                key={item.path}
                                path={item.path}
                                label={item.label}
                                icon={item.icon} />
                            <style global jsx>{`
                                .NavButton.active{
                                    background-color: #000000 !important;
                                    color: white !important;
                                }
                                .NavButton.active span.icon{
                                    color: white !important;
                                }
                                `}</style>
                        </Grid>
                    ))}
                </Grid.Container>
            </Grid>
            <Grid xs={24} xl={23}>
                <Grid.Container>
                    <Grid xs={24}>
                        <Row style={{ padding: '10px', boxShadow: '0px 2px 10px 0px rgba(203,232,255,0.2)', 
                        backgroundColor: 'white', width: '100%', zIndex: '999' }}>
                            <Spacer x={1} />
                            <Text style={{ textAlign: 'center' }} h3>eVoting Admin Prototype</Text>
                        </Row>
                        <div style={{padding: '24px' }}>
                        {props.children}
                        </div>
                    </Grid>
                </Grid.Container>
            </Grid>
        </Grid.Container>
    )
}

export default NavBar;