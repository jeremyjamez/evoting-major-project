import useSWR from "swr";

const fetcher = (...args) => fetch(...args).then(res => res.json());
const baseUrl = process.env.apiUrl

export const useMembers = (path) => {
    const { data: members, error } = useSWR(`${baseUrl}${path}`)
    return {
        members,
        isError: error
    }
};

export const useCandidates = (path) => {
    const { data: candidates, error } = useSWR(`${baseUrl}${path}`)
    return {
        candidates,
        isError: error
    }
}

export const useElections = (path) => {
    const { data: elections, error } = useSWR(`${baseUrl}${path}`)
    return {
        elections,
        isError: error
    }
};

export const usePoliticalParties = (path) => {
    const { data: parties, error } = useSWR(`${baseUrl}${path}`)
    return {
        parties,
        isError: error
    }
};

export const useVoters = (path) => {
    const { data: voters, error } = useSWR(`${baseUrl}${path}`)
    return {
        voters,
        isError: error
    }
};

export const useConstituencies = (path) => {
    const { data: constituencies, error } = useSWR(`${baseUrl}${path}`)
    return {
        constituencies,
        isError: error
    }
}

export const useMP = (path) => {
    const { data: mps, error } = useSWR(`${baseUrl}${path}`)
    return {
        mps,
        isError: error
    }
}

export const useConstituencyMP = (name) => {
    const { data: constituency } = useSWR(`${baseUrl}/Constituencies/GetByName/${name}`)
    const { data: candidate } = useSWR(() => `${baseUrl}/Candidates/GetByConstituency/${constituency.constituencyId}`)
    const { data: mp } = useSWR(() => `${baseUrl}/MemberOfParliaments/${constituency.constituencyId}/${candidate.candidateId}`)
    const { data: member } = useSWR(() => `${baseUrl}/Members/${candidate.candidateId}`)
    const { data: voters } = useSWR(() => `${baseUrl}/voters/GetByConstituencyId/${constituency.constituencyId}`)

    return {
        constituency,
        candidate,
        mp,
        member,
        voters
    }
}

export const usePollingStations = () => {

}

export const useSecurityQuestions = (voterId) => {
    const { data: questions, error } = useSWR(`${baseUrl}/voters/GetSecurityQuestionsById/${voterId}`)

    return {
        questions,
        isError: error
    }
}