using BlogAngApi.Model;
using BlogAngApi.Models;
using BlogAngApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogAngApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public LoginController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest model)
        {
            Console.WriteLine($"Login attempt: {model.email}");

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid request" });
            }

            var authResult = _userService.UserAuth(model.email, model.Password);
            if (!authResult)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var user = _userService.GetUserbyEmail(model.email);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("AuthenticationType", "user")
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(60);  

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine("Giriş Başarılı");
            return Ok(new
            {
                message = "Login successful",
                token = tokenString
            });
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterLoginViewModel model)
        {
            

            if (model is not null)
            {
                User eklenecek = new User()
                {
                    Email = model.email,
                    Username = model.username,
                    PasswordHash = model.password,
                };
                _userService.AddUser(eklenecek);
                return Ok(new { message = "Registration successful. Please login." });
            }

            return BadRequest(new { message = "Invalid data" });
        }

        [HttpPost("logout")]
        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("user");
            return Ok(new { message = "Logout successful" });
        }
    }
}
