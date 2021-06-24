using eVotingApi.Data;
using eVotingApi.Models.Auth;
using eVotingApi.Models.DTO;
using eVotingApi.Models.DTO.Requests;
using eVotingApi.Models.DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly eVotingContext _eVotingContext;

        public UsersController(UserManager<ApplicationUser> userManager, eVotingContext eVotingContext)
        {
            _userManager = userManager;
            _eVotingContext = eVotingContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            return await _eVotingContext.Users.Select(x => UserToDto(x)).ToListAsync();
        }

        [HttpGet]
        [Route("Roles")]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetRoles()
        {
            return await _eVotingContext.Roles.ToListAsync();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto user)
        {
            // Check if the incoming request is valid
            if (ModelState.IsValid)
            {
                // check if the user with the same username exist
                var existingUser = await _userManager.FindByNameAsync(user.UserName);

                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Success = false,
                        Errors = new List<string>(){
                            "Username already exist"
                        }
                    });
                }

                var newUser = new ApplicationUser()
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    TRN = user.TRN,
                    Role = user.Role,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email
                };

                var isCreated = await _userManager.CreateAsync(newUser, user.Password);

                if (isCreated.Succeeded)
                {
                    var isAddToRole = await _userManager.AddToRoleAsync(newUser, user.Role);

                    if (isAddToRole.Succeeded)
                    {
                        return Ok();
                    }
                }

                return new JsonResult(new RegistrationResponse()
                {
                    Success = false,
                    Errors = isCreated.Errors.Select(x => x.Description).ToList()
                }
                )
                { StatusCode = 500 };
            }

            return BadRequest(new RegistrationResponse()
            {
                Success = false,
                Errors = new List<string>(){
                    "Invalid payload"
                }
            });
        }

        private static UserDto UserToDto(ApplicationUser user) =>
            new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Address = user.Address,
                TRN = user.TRN,
                Role = user.Role,
                LastLoggedIn = user.LastLoggedIn,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
    }
}
