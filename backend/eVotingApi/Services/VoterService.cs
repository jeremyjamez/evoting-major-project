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
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Services
{
    public class VoterService
    {
        private readonly IMongoCollection<Voter> _voters;
        private readonly IConfiguration _config;
        private const string RECOGNITION_MODEL4 = RecognitionModel.Recognition04;

        public VoterService(IEVotingDatabaseSettings settings, IConfiguration config)
        {
            _config = config;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _voters = database.GetCollection<Voter>(settings.VoterCollectionName);
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
                if (!result.isTwoFactorEnabled)
                {
                    registeredResponse = new RegisteredResponse
                    {
                        isRegistered = true,
                        isTwoFactorEnabled = false
                    };
                    return registeredResponse;
                }
                else
                {
                    registeredResponse = new RegisteredResponse
                    {
                        isRegistered = true,
                        isTwoFactorEnabled = true
                    };
                    return registeredResponse;
                }
            }

            return new RegisteredResponse { isRegistered = false, isTwoFactorEnabled = false };
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
        private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, string url, string recognition_model)
        {
            IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithUrlAsync(url, recognitionModel: recognition_model, detectionModel: DetectionModel.Detection03);
            return detectedFaces.ToList();
        }

        /// <summary>
        /// Verifys the voters identity by comparing their photo stored in the system to the one they provide during verification
        /// </summary>
        /// <param name="voterId"></param>
        /// <param name="sourceImgUrl"></param>
        /// <returns>VerifyResult</returns>
        public async Task<VerifyResult> VerifyVoterIdentity(string voterId, string sourceImgUrl)
        {
            IFaceClient client = Authenticate(_config.GetValue<string>("AzureFaceConfig:Endpoint"), _config.GetValue<string>("AzureFaceConfig:Key"));
            var filter = Builders<Voter>.Filter.Eq("voterId", voterId);
            var photoUrl = await _voters.Find(filter).Project(v => v.Photo).FirstOrDefaultAsync();

            List<DetectedFace> targetDetectedFaces = await DetectFaceRecognize(client, photoUrl, RECOGNITION_MODEL4);
            Guid targetFaceId = targetDetectedFaces[0].FaceId.Value;

            List<DetectedFace> sourceDetectedFaces = await DetectFaceRecognize(client, sourceImgUrl, RECOGNITION_MODEL4);
            Guid sourceFaceId = sourceDetectedFaces[0].FaceId.Value;

            VerifyResult result = await client.Face.VerifyFaceToFaceAsync(sourceFaceId, targetFaceId);

            return result;
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
