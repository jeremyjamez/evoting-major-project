import { Archive, CheckSquare, Home, MapPin, UserPlus, Users } from "@geist-ui/react-icons";

export const navMenu = [
    {
        label: "Home",
        path: "/admin/home",
        icon: <Home />
    },
    {
        label: "Elections",
        path: "/admin/elections",
        icon: <Archive />
    },
    {
        label: "Candidates",
        path: "/admin/candidates",
        icon: <Users />
    },
    {
        label: "Voters List",
        path: "/admin/voters",
        icon: <Users />
    },
    {
        label: "Constituency",
        path: "/admin/constituencies",
        icon: <MapPin/>
    },
    {
        label: 'Political Parties',
        path: "/admin/political-parties",
        icon: <CheckSquare/>
    },
    {
        label: 'Users',
        path: '/admin/users',
        icon: <UserPlus/>
    }
];