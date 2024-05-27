using blazchat.Entities;
using blazchat.Services;
using Microsoft.AspNetCore.Mvc;

namespace blazchat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto request)
        {
            User user = new User
            {
                Id = request.Id,
                Name = request.Name
            };

            var userId = await _userService.CreateUser(user);
            return Ok(userId);
        }

        [HttpGet("getUser/userId={userId:guid}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var user = await _userService.GetUser(userId);
            return Ok(user);
        }

        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

    }
}
