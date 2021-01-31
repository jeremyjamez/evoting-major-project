import NavBar from "./components/NavBar";

const DashboardLayout = ({ children }) => {

    return (
        <>
            <NavBar>
                {children}
            </NavBar>

            <style global jsx>{`
            body {
                background-color: white !important;
            }
        `}</style>
        </>
    )
}

export default DashboardLayout;