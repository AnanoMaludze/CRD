using CRD.Enums;
using CRD.Interfaces;
using CRD.Models;
using CRD.Utils;
using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace CRD.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;


        public AuthService(IConfiguration configuration)
        {

            _configuration = configuration;
        }
        public string CreateToken(CreateTokenModel user)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserID.ToString()),
                new Claim(ClaimTypes.Role, "User")
            };

            string symKey = _configuration.GetSection("AppSettings:Token").Value!;


            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            symKey));


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
