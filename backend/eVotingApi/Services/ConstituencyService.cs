using eVotingApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Services
{
    public class ConstituencyService
    {
        private readonly IMongoCollection<Constituency> _constituencies;

        public ConstituencyService(IEVotingDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _constituencies = database.GetCollection<Constituency>(settings.ConstituencyCollectionName);
        }

        public async Task<IEnumerable<Constituency>> Get()
        {
            return await _constituencies.Find(constituency => true).ToListAsync();
        }

        public async Task<Constituency> GetById(string id)
        {
            return await _constituencies.Find(constituency => constituency.ConstituencyId == id).FirstOrDefaultAsync();
        }

        public async Task<Constituency> GetByName(string name)
        {
            return await _constituencies.Find(constituency => constituency.Name.Equals(name)).FirstOrDefaultAsync();
        }
    }
}
