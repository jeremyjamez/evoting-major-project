﻿using eVotingApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Services
{
    public class ElectionService
    {
        private readonly IMongoCollection<Election> _elections;

        public ElectionService(IEVotingDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _elections = database.GetCollection<Election>(settings.ElectionCollectionName);
        }

        public async Task<IEnumerable<Election>> GetElections()
        {
            return await _elections.Find(election => true).ToListAsync();
        }

        public async Task<Election> GetElection(string electionId)
        {
            var builder = Builders<Election>.Filter;
            var filter = builder.Eq("_id", ObjectId.Parse(electionId));
            var election = await _elections.Find(filter).FirstOrDefaultAsync();
            return election;
        }

        public async Task<string> GetByTime(long time)
        {
            var startTimeFilter = Builders<Election>.Filter.Lte("startTime", time);
            var endTimeFilter = Builders<Election>.Filter.Gte("endTime", time);
            var filter = Builders<Election>.Filter.And(startTimeFilter, endTimeFilter);
            var electionId = await _elections.Find(filter).Project(e => e.Id).FirstOrDefaultAsync();

            return electionId;
        }

        public async Task<string> AddElection(Election election)
        {
            try
            {
                await _elections.InsertOneAsync(election);
                return election.Id;
            }
            catch (MongoWriteException e)
            {
                Debug.Write(e);
                return null;
            }
            catch(Exception e)
            {
                Debug.Write(e);
                return null;
            }
        }

        public async Task<DeleteResult> Delete(string electionId)
        {
            var builder = Builders<Election>.Filter;
            var filter = builder.Eq("electionId", electionId);
            var result = await _elections.DeleteOneAsync(filter);

            return result;
        }
    }
}
