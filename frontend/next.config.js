
module.exports = {
    
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