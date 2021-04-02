import { Archive, Check, CheckSquare, Home, Layout, MapPin, UserPlus, Users } from "@geist-ui/react-icons";

const navMenu = [
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
        label: "Members",
        path: "/admin/members",
        icon: <Users />
    },
    {
        label: "Voters",
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

export default navMenu;