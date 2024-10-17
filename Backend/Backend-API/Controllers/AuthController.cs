using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Backend_API.Models;

namespace Backend_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var userExists = await _userManager.FindByNameAsync(userDto.Name);
            if (userExists != null)
                return BadRequest("User already exists.");

            var user = new IdentityUser
            {
                UserName = userDto.Name,
                Email = userDto.Name
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            var result = await _signInManager.PasswordSignInAsync(userDto.Name, userDto.Password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid credentials.");
            }

            var user = await _userManager.FindByNameAsync(userDto.Name);
            return Ok(new
            {
                userId = user.Id,
                name = user.UserName
            });
        }

    }
}
