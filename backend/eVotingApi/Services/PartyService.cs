using eVotingApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Services
{
    public class PartyService
    {
        private readonly IMongoCollection<Party> _parties;

        public PartyService(IEVotingDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _parties = database.GetCollection<Party>(settings.PartyCollectionName);
        }

        public async Task<IEnumerable<Party>> GetParties()
        {
            return await _parties.Find(party => true).ToListAsync();
        }

        public async Task<Party> GetParty(string partyId)
        {
            var builder = Builders<Party>.Filter;
            var filter = builder.Eq("partyId", partyId);
            var party = await _parties.Find(filter).FirstOrDefaultAsync();
            return party;
        }
    }
}
