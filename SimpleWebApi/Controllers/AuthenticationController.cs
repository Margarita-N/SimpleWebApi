using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SimpleWebApi.Logic.Interfaces;
using SimpleWebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;

namespace SimpleWebApi.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly string SigningKey;
        private readonly string Issuer;
        private readonly string Audience;

        private readonly TimeSpan TokenLifeSpan = TimeSpan.FromHours(1);

        private readonly IUserRepository _userRepository;

        public AuthenticationController(IUserRepository userService, IConfiguration configuration)
        {
            _userRepository = userService;
            SigningKey = configuration["Authorization:SigningKey"]!;
            Issuer = configuration["Authorization:Issuer"]!;
            Audience = configuration["Authorization:Audience"]!;
        }

        [HttpPost("token")]
        public ActionResult GenerateToken([FromBody]TokenRequest request)
        {
            var isValidUser = _userRepository.IsValidUser(request.Email, request.Password);

            if (isValidUser)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var keyBytes = Encoding.UTF8.GetBytes(SigningKey);

                var user = _userRepository.GetUser(request.Email);
                var userRoles = user.Roles.Select(x => x.UserRole.Name);
                var serializedRoles = JsonConvert.SerializeObject(userRoles);

                var tokenClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, request.Email),
                    new Claim(JwtRegisteredClaimNames.Email, request.Email),
                    new Claim("Roles",serializedRoles)
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(tokenClaims),
                    Expires = DateTime.UtcNow.Add(TokenLifeSpan),
                    Issuer = Issuer,
                    Audience = Audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(tokenHandler.WriteToken(token));
            }

            return BadRequest();
        }
    }
}
