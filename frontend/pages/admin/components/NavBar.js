import { Grid, Text, Tooltip, Link } from "@geist-ui/react";
import menuItems from "./navMenu";
import { default as NextLink } from 'next/link'
import { withRouter } from "next/router";

const NavBar = props => {
    return (
        <Grid.Container style={{ flex: '1 1 auto' }}>
            <Grid
                xl={1}
                style={{
                    borderRight: "1px solid #e2e2e2",
                    backgroundColor: "#f8f8f8",
                    color: "white",
                    paddingLeft: "8px",
                    paddingRight: "8px"
                }}
            >
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
                <div style={{ padding: "10px", boxShadow: "0px 2px 10px 0px rgba(203,232,255,0.2)", backgroundColor: "white", zIndex: "999", width: '100%', flex: '0 1 auto' }}>
                    <Text style={{ paddingLeft: '16px' }} h3>eVoting Admin Prototype</Text>
                </div>
                <div style={{flex: '1 1 auto'}}>{props.children}</div>
            </Grid>
            <style global jsx>{`
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
                    border-radius: 4px !important;
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
        </Grid.Container>
    );
}

export default withRouter(NavBar);