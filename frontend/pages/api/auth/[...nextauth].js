import NextAuth from 'next-auth'
import Providers from 'next-auth/providers'
import https from "https"

const options = {
    providers: [
        Providers.Credentials({
            authorize: async (credentials) => {
                
                // Add logic here to look up the user from the credentials supplied
                const httpsAgent = new https.Agent({
                    rejectUnauthorized: false,
                });

                console.log(credentials.password)

                fetch(`${process.env.apiUrl}/authmanagement/login`, 
                {
                    agent: httpsAgent,
                    method: 'POST',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                    },
                    body: {
                        "username": credentials.username,
                        "password": credentials.password
                    }
                })
                .then(res => {
                    return res.url
                })
                .then(data => {
                    console.log(data)
                })
                //const user = await res.json()

                /* console.log("Credentials: " + JSON.stringify(credentials))

                if (user) {
                    // Any object returned will be saved in `user` property of the JWT
                    return Promise.resolve(user)
                } else {
                    // If you return null or false then the credentials will be rejected
                    return Promise.resolve(null)
                    // You can also Reject this callback with an Error or with a URL:
                    // return Promise.reject(new Error('error message')) // Redirect to error page
                    // return Promise.reject('/path/to/redirect')        // Redirect to a URL
                } */
            }
        })
    ],
    pages: {
        signIn: '/admin/login'
    },
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