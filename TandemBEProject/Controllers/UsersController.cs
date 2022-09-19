using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TandemBEProject.DAL.Exceptions;
using TandemBEProject.DTOs;
using TandemBEProject.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TandemBEProject.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmail([FromQuery][Required][EmailAddress] string email)
        {
            UserResponseDto? userByEmail = await _usersService.GetUserByEmail(email);

            return userByEmail == null ? NotFound() : Ok(userByEmail);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto createUserRequest)
        {
            try
            {
                UserResponseDto createdUser = await _usersService.CreateUser(createUserRequest);

                return CreatedAtAction(nameof(GetUserByEmail), new { email = createdUser.EmailAddress }, createdUser);
            }
            catch (UserExistsException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
