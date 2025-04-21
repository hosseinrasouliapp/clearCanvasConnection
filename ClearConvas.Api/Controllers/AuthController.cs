using ClearConvas.Infrastructure.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace ClearConvas.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthController(IConfiguration config)
        {
            var jwtSettings = config.GetSection("JwtSettings");
            _tokenGenerator = new JwtTokenGenerator(
                jwtSettings["SecretKey"],
                jwtSettings["Issuer"],
                jwtSettings["Audience"]
            );
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // for test
            if (request.Username == "admin" && request.Password == "admin")
            {
                var token = _tokenGenerator.GenerateToken(request.Username, "Admin");
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password.");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
