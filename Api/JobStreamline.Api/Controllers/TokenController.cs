using JobStreamline.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobStreamline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _iConfiguration;

        public TokenController(IConfiguration Configuration)
        {
            _iConfiguration = Configuration;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] LoginModel loginModel)
        {
            if (IsValidUser(loginModel))
            {
                var token = GenerateToken(loginModel.Username);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        private bool IsValidUser(LoginModel loginModel)
        {
            return loginModel.Username == _iConfiguration["User:Name"] && loginModel.Password == _iConfiguration["User:Password"];
        }

        private string GenerateToken(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iConfiguration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _iConfiguration["Jwt:Issuer"],
                audience: _iConfiguration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


}
