import useSWR from "swr";
import axios from 'axios'

export const fetcher = (url, token) =>
    axios.get(url, {headers:{Authorization: 'Bearer ' + token}}).then(res => res.data)


const baseUrl = process.env.NEXT_PUBLIC_API_URL

export const useMembers = (token) => {
    const { data: members, error } = useSWR([`${baseUrl}/members`,token], fetcher)
    return {
        members,
        isError: error
    }
};

export const useCandidates = (token) => {
    const { data: candidates, error } = useSWR([`${baseUrl}/candidates`,token], fetcher)
    return {
        candidates,
        isError: error
    }
}

export const useElections = (token) => {
    const { data: elections, error } = useSWR([`${baseUrl}/elections`,token], fetcher, {revalidateOnFocus: false, refreshInterval: 0})
    return {
        elections,
        isError: error
    }
};

export const useGetElection = (token, currentTime) => {
    const { data: electionId } = useSWR([`${baseUrl}/elections/getbytime/${currentTime}`,token], fetcher, {revalidateOnFocus: false, refreshInterval: 0})
    const { data: election, error } = useSWR(() => [`${baseUrl}/elections/${electionId}`,token], fetcher, {revalidateOnFocus: false})
    return {
        election,
        isError: error
    }
}

export const usePoliticalParties = (token) => {
    const { data: parties, error } = useSWR([`${baseUrl}/politicalparties`,token], fetcher)
    return {
        parties,
        isError: error
    }
};

export const useVoters = (token) => {
    const { data: voters, error } = useSWR([`${baseUrl}/voters`,token], fetcher)
    return {
        voters,
        isLoading: !error && !voters,
        isError: error
    }
};

export const useConstituencies = (token) => {
    const { data: constituencies, error } = useSWR([`${baseUrl}/constituencies`,token], fetcher)
    return {
        constituencies,
        isError: error
    }
}

export const useConstituency = (name,token) => {
    const { data: constituency } = useSWR([`${baseUrl}/Constituencies/GetConstituencyByName/${name}`,token], fetcher)
    const { data: candidate } = useSWR(() => [`${baseUrl}/Candidates/GetByConstituency/${constituency.constituencyId}`,token], fetcher)
    const { data: voters } = useSWR(() => [`${baseUrl}/voters/GetByConstituencyId/${constituency.constituencyId}`,token], fetcher)

    return {
        constituency,
        candidate,
        voters
    }
}

export const useUsers = (token) => {
    const { data: users, error } = useSWR([`${baseUrl}/users`, token], fetcher)

    return {
        users,
        isLoading: !error && !users,
        isError: error
    }
}

export const useRoles = (token) => {
    const { data: roles, error } = useSWR([`${baseUrl}/users/roles`, token], fetcher)

    return {
        roles,
        isLoading: !error && !roles,
        isError: error
    }
}

export const usePair = (voterId) => {
    const {data: qr, error} = useSWR(`${baseUrl}/voters/pair/${voterId}`, fetcher, {revalidateOnFocus: false})

    return {
        qr,
        isLoading: !error && !qr,
        isError: error
    }
}

export const useGetParty = (affiliation, token) => {
    const {data: party, error} = useSWR([`${baseUrl}/politicalparties/${affiliation}`, token], fetcher, {revalidateOnFocus: false})

    return {
        party,
        isLoading: !error && !party,
        isError: error
    }
}

export const useGetVoterElection = (token) => {
    const {data: voters, error} = useSWR([`${baseUrl}/voters/getvoterelections`, token], fetcher, {revalidateOnFocus: false})

    return {
        voters,
        isLoading: !error && !voters,
        isError: error
    }
}