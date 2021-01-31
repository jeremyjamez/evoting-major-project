import { Button, Link, Text, Tooltip } from "@geist-ui/react";
import { withRouter } from "next/router";
import "./NavButton.module.css";

const NavButton = props => (
    <Link href={props.path} key={props.path}>
        <Tooltip text={props.label} placement="right" type="dark">
            <Button
                auto
                shadow={props.router.pathname === props.path ? true : false}
                size="medium"
                className={`NavButton ${props.router.pathname === props.path ? "active" : ""}`}
                iconRight={props.icon}>
                {props.drawerSize === 4 ? <Text h5>{props.label}</Text> : null}
            </Button>
        </Tooltip>
    </Link>
);

export default withRouter(NavButton);