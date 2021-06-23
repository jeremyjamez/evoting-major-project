using eVotingApi.Models;
using eVotingApi.Models.DTO;
using eVotingApi.Models.DTO.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eVotingApi.Services
{
    public class VoterService
    {
        private readonly IMongoCollection<Voter> _voters;
        private readonly IMongoCollection<Vote> _votes;
        private readonly IMongoCollection<Election> _elections;
        private readonly IConfiguration _config;
        private const string RECOGNITION_MODEL4 = RecognitionModel.Recognition04;

        public VoterService(IEVotingDatabaseSettings settings, IConfiguration config)
        {
            _config = config;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _voters = database.GetCollection<Voter>(settings.VoterCollectionName);
            _votes = database.GetCollection<Vote>(settings.VoteCollectionName);
            _elections = database.GetCollection<Election>(settings.ElectionCollectionName);
        }

        public string GetPublicKey()
        {
            return _config.GetValue<string>("PublicKey:Key");
        }

        /// <summary>
        /// Returns a list of all voters
        /// </summary>
        /// <returns>An enumerable of voters</returns>
        public async Task<IEnumerable<VoterDto>> Get()
        {
            return await _voters.Find(voter => true).Project(x => VoterToDto(x)).ToListAsync();
        }

        /// <summary>
        /// Returns a voter specified by the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>VoterDto</returns>
        public async Task<VoterDto> GetById(string id)
        {
            return await _voters.AsQueryable().Where(voter => voter.VoterId == id).Select(voter => VoterToDto(voter)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Checks if the voter which is specified by the ID and date of birth, is registered
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dob"></param>
        /// <returns>A RegisteredResponse object containing boolean values if the voter is registered and has 2FA enabled</returns>
        public async Task<RegisteredResponse> IsRegistered(VoterDto voterDto)
        {
            var builder = Builders<Voter>.Filter;
            var filter = builder.And(builder.Eq("voterId", voterDto.VoterId), builder.Eq("dateofBirth", DateTime.Parse(voterDto.DateofBirth).Date.ToString("dd/MM/yyyy")));
            var result = await _voters.Find(filter).FirstOrDefaultAsync();

            RegisteredResponse registeredResponse;

            if (result != null)
            {
                var voteBuilder = Builders<Vote>.Filter;

                var electionId = await GetByTime(voterDto.CurrentTime);

                var voteFilter = voteBuilder.And(voteBuilder.Eq("voterId", result.VoterId), voteBuilder.Eq("electionId", electionId));
                var hasVoted = await _votes.Find(voteFilter).FirstOrDefaultAsync();

                if(hasVoted != null)
                {
                    registeredResponse = new RegisteredResponse
                    {
                        IsRegistered = true,
                        HasVoted = true
                    };
                    return registeredResponse;
                }

                string folderName = Path.Combine("wwwroot", "certs");
                string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                using(StreamWriter sw = new StreamWriter(Path.Combine(pathToSave, string.Format("{0}_publickey.pem", voterDto.VoterId))))
                {
                    await sw.WriteAsync(voterDto.PublicKey);
                }

                if (!result.isTwoFactorEnabled)
                {
                    registeredResponse = new RegisteredResponse
                    {
                        IsRegistered = true,
                        IsTwoFactorEnabled = false,
                        PublicKey = _config.GetValue<string>("PublicKey:Key")
                    };
                    return registeredResponse;
                }
                else
                {
                    registeredResponse = new RegisteredResponse
                    {
                        IsRegistered = true,
                        IsTwoFactorEnabled = true,
                        PublicKey = _config.GetValue<string>("PublicKey:Key")
                    };
                    return registeredResponse;
                }
            }

            return new RegisteredResponse { IsRegistered = false, IsTwoFactorEnabled = false };
        }

        private async Task<string> GetByTime(long time)
        {
            var startTimeFilter = Builders<Election>.Filter.Lte("startTime", time);
            var endTimeFilter = Builders<Election>.Filter.Gte("endTime", time);
            var filter = Builders<Election>.Filter.And(startTimeFilter, endTimeFilter);
            var electionId = await _elections.Find(filter).Project(e => e.Id).FirstOrDefaultAsync();

            return electionId;
        }

        /// <summary>
        /// Returns all voters within a constituency specified by the ID
        /// </summary>
        /// <param name="constituencyId"></param>
        /// <returns>An enumerable of voters</returns>
        public async Task<IEnumerable<Voter>> GetByConstituencyId(string constituencyId)
        {
            return await _voters.Find(voter => voter.ConstituencyId == constituencyId).ToListAsync();
        }

        /// <summary>
        /// Retrieves a voter specified by the ID and returns a Data Transfer Object (DTO) containing data for security questions
        /// </summary>
        /// <param name="id"></param>
        /// <returns>SecurityQuestionsDTO</returns>
        public async Task<SecurityQuestionsDTO> GetQuestions(string id)
        {
            var builder = Builders<Voter>.Filter;
            var filter = builder.Eq("voterId", id);

            return await _voters.Find(filter).Project(v => QuestionsToDTO(v)).FirstOrDefaultAsync();
        }

        /*public async Task CheckAnswer(string answer, string voterId)
        {
            var builder = Builders<Voter>.IndexKeys;
            var indexOptions = new CreateIndexOptions<Voter>();
            indexOptions
                .WildcardProjection
                .Include("mothersMaidenName")
                .Include("occupation")
                .Include("telephoneNumber");
            var indexModel = new CreateIndexModel<Voter>(builder.Wildcard("$**"), options: indexOptions);

            await _voters.Indexes.CreateOneAsync(indexModel);
            
            var filter = builder.Eq("voterId", voterId);
            builder.Text(answer, options: [])
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salt"></param>
        /// <param name="hashedSecretCode"></param>
        /// <param name="voterId"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateHashedSecretCode(string salt, string voterId)
        {
            var filter = Builders<Voter>.Filter.Eq("voterId", voterId);
            var update = Builders<Voter>.Update.Set("salt", salt).Set("isTwoFactorEnabled", true);
            var result = await _voters.UpdateOneAsync(filter, update);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voterId"></param>
        /// <returns></returns>
        public async Task<string> GetSecretCodeSalt(string voterId)
        {
            var filter = Builders<Voter>.Filter.Eq("voterId", voterId);
            return await _voters.Find(filter).Project(v => v.Salt).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Authenticate the Face Client with the Azure Face API Subscription
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="key"></param>
        /// <returns>IFaceClient</returns>
        private static IFaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        /// <summary>
        /// Detects faces from the provided image url
        /// </summary>
        /// <param name="faceClient"></param>
        /// <param name="url"></param>
        /// <param name="recognition_model"></param>
        /// <returns>List of detected faces</returns>
        private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, byte[] imageBytes, string recognition_model)
        {
            try
            {
                using (var stream = new MemoryStream(imageBytes))
                {
                    IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithStreamAsync(stream, recognitionModel: recognition_model, detectionModel: DetectionModel.Detection03);
                    return detectedFaces.ToList();
                }
            }
            catch(APIErrorException e)
            {
                return null;
            }
            
        }

        /// <summary>
        /// Verifys the voters identity by comparing their photo stored in the system to the one they provide during verification
        /// </summary>
        /// <param name="voterId"></param>
        /// <param name="sourceImgUrl"></param>
        /// <returns>VerifyResult</returns>
        public async Task<VerifyResult> VerifyVoterIdentity(string voterId, byte[] sourceImgBytes)
        {
            IFaceClient client = Authenticate(_config.GetValue<string>("AzureFaceConfig:Endpoint"), _config.GetValue<string>("AzureFaceConfig:Key"));
            var filter = Builders<Voter>.Filter.Eq("voterId", voterId);
            var photoUrl = await _voters.Find(filter).Project(v => v.Photo).FirstOrDefaultAsync();

            List<DetectedFace> sourceDetectedFaces = await DetectFaceRecognize(client, sourceImgBytes, RECOGNITION_MODEL4);

            if (sourceDetectedFaces == null)
                return new VerifyResult(false, 0);

            Guid sourceFaceId = sourceDetectedFaces[0].FaceId.Value;

            using (var webClient = new WebClient())
            {
                byte[] imageBytes = webClient.DownloadData(photoUrl);

                List<DetectedFace> targetDetectedFaces = await DetectFaceRecognize(client, imageBytes, RECOGNITION_MODEL4);

                Guid targetFaceId = targetDetectedFaces[0].FaceId.Value;

                VerifyResult result = await client.Face.VerifyFaceToFaceAsync(sourceFaceId, targetFaceId);

                return result;

            }            
        }

        /// <summary>
        /// Data Transfer Object (DTO) which hides some properties of the Voter entity
        /// </summary>
        /// <param name="voter"></param>
        /// <returns></returns>
        private static VoterDto VoterToDto(Voter voter) =>
            new VoterDto
            {
                VoterId = voter.VoterId,
                FirstName = voter.FirstName,
                MiddleName = voter.MiddleName,
                LastName = voter.LastName,
                DateofBirth = voter.DateOfBirth
            };

        /// <summary>
        /// Data Transfer Object (DTO) which hides some properties of the Voter entity to
        /// only contain data for generating security questions
        /// </summary>
        /// <param name="voter"></param>
        /// <returns></returns>
        private static SecurityQuestionsDTO QuestionsToDTO(Voter voter) =>
            new SecurityQuestionsDTO
            {
                Address = voter.Address,
                Telephone = voter.Telephone,
                Occupation = voter.Occupation,
                MothersMaidenName = voter.MothersMaidenName,
                PlaceOfBirth = voter.PlaceOfBirth,
                MothersPlaceOfBirth = voter.MothersPlaceOfBirth
            };
    }
}
