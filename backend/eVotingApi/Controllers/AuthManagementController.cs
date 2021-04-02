﻿using eVotingApi.Config;
using eVotingApi.Data;
using eVotingApi.Domain;
using eVotingApi.Models;
using eVotingApi.Models.Auth;
using eVotingApi.Models.DTO.Requests;
using eVotingApi.Models.DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eVotingApi.Controllers
{
    [Route("api/AuthManagement")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthManagementController(
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                // check if the user with the same email exist
                var existingUser = await _userManager.FindByNameAsync(user.UserName);

                if (existingUser == null)
                {
                    // We dont want to give to much information on why the request has failed for security reasons
                    return BadRequest(new RegistrationResponse()
                    {
                        Success = false,
                        Errors = new List<string>(){
                            "Invalid authentication request"
                        }
                    });
                }

                // Now we need to check if the user has inputed the right password
                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (!isCorrect)
                {
                    // We dont want to give to much information on why the request has failed for security reasons
                    return BadRequest(new RegistrationResponse()
                    {
                        Success = false,
                        Errors = new List<string>(){
                            "Invalid authentication request"
                        }
                    });
                }

                var jwtToken = GenerateJwtToken(existingUser);

                string zoneId = "Eastern Standard Time";
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
                DateTime result = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);

                existingUser.LastLoggedIn = result;

                await _userManager.UpdateAsync(existingUser);

                return Ok(jwtToken);
            }

            return BadRequest(new RegistrationResponse()
            {
                Success = false,
                Errors = new List<string>(){
                   "Invalid payload"
                }
            });
        }

        private AuthResult GenerateJwtToken(ApplicationUser user)
        {
            // Now its time to define the jwt token which will be responsible of creating our tokens
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // We get our secret from the appsettings
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            // we define our token descriptor
            // We need to utilise claims which are properties in our token which gives information about the token
            // which belong to the specific user who it belongs to
            // so it could contain their id, name, email the good part is that these information
            // are generated by our server and identity framework which is valid and trusted
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.UserName),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                // here we are adding the encryption alogorithim information which will be used to decrypt our token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return new AuthResult()
            {
                Token = jwtToken,
                Success = true
            };
        }
    }
}
