using BlogAngApi.Model;
using BlogAngApi.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginRequest = BlogAngApi.Model.LoginRequest;

namespace BlogAngApi.Controllers
{
    [Route("api/admin/auth")]
    [ApiController]
    public class AdminAuthController : ControllerBase
    {
        private readonly IAdminServices  _adminServices;
        private readonly IConfiguration _configuration;

        public AdminAuthController(IAdminServices adminServices, IConfiguration configuration)
        {
            _adminServices = adminServices;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (_adminServices.AdminAuth(request.email, request.Password))
            {
                // JWT token oluşturma
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, request.email),
            new Claim(ClaimTypes.Role, _adminServices.GetRoleByEmail(request.email))
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new
                {
                    success = true,
                    token = tokenString,
                    message = "Giriş başarılı"
                });
            }

            return Unauthorized(new { success = false, message = "Kullanıcı adı veya şifre yanlış!" });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { success = true, message = "Çıkış yapıldı" });
        }
    }
}
