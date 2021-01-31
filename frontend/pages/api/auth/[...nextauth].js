import NextAuth from 'next-auth'
import Providers from 'next-auth/providers'

const options = {
    providers: [
        Providers.Credentials({
            name: "Voters Identification",
            credentials: {
                id: { label: "Voters ID", type: "text", placeholder: "Voters ID" }
            },
            authorize: async (credentials) => {
                // Add logic here to look up the user from the credentials supplied
                const user = { id: 1, name: 'J Smith', email: 'jsmith@example.com' }

                if (user) {
                    // Any object returned will be saved in `user` property of the JWT
                    return Promise.resolve(user)
                } else {
                    // If you return null or false then the credentials will be rejected
                    return Promise.resolve(null)
                    // You can also Reject this callback with an Error or with a URL:
                    // return Promise.reject(new Error('error message')) // Redirect to error page
                    // return Promise.reject('/path/to/redirect')        // Redirect to a URL
                }
            }
        })
    ],
}

export async function getStaticProps() {
    const res = await fetch("https://localhost:5001/api/Elections");
    const elections = await res.json();
    return {
        props: {
            elections,
        },
    }
}

export default (req, res) => NextAuth(req, res, options)