using Microsoft.AspNetCore.Mvc;
using SkillCypher.Core.DTOs.Auth;
using SkillCypher.Core.Interfaces;

namespace SkillCypher.API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var res = await _authService.RegisterAsync(registerDto);

        if(res == null)
            {
                return Conflict(new
                {
                    message = "A User with this email already exists"
                });
            }
            return CreatedAtAction(nameof(Register),res);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var res = await _authService.LoginAsync(loginDto);
            if(res == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }
            return Ok(res);
        }
    }
}