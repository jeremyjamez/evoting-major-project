module.exports = {
    env: {
        apiUrl: 'https://localhost:44387/api'
    },
    async redirects() {
        return [
            {
                source: '/admin',
                destination: '/admin/login',
                permanent: true
            }
        ]
    }
}