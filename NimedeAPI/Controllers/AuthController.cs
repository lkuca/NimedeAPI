using Microsoft.AspNetCore.Mvc;
using NimedeAPI.Modules; // Update with your namespace
using System.Linq;
using NimedeAPI.Modules;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private static List<User> users = new List<User>();

        // POST: /auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (users.Any(u => u.Username == request.Username))
            {
                return BadRequest("Username already exists.");
            }

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            users.Add(newUser);
            return Ok("Registration successful!");
        }

        // POST: /auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = users.FirstOrDefault(u => u.Username == request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok("Login successful!");
        }

        // POST: /auth/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Add token invalidation logic if using tokens
            return Ok("Logout successful!");
        }
    }
}
