using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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
        public async Task<IActionResult> Post([FromQuery][Required][EmailAddress] string email)
        {
            UserResponseDto? userByEmail = await _usersService.GetUserByEmail(email);

            return userByEmail == null ? NotFound() : Ok(userByEmail);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserRequestDto createUserRequest)
        {
            UserResponseDto createdUser = await _usersService.CreateUser(createUserRequest);

            return Ok(createdUser);
        }
    }
}
