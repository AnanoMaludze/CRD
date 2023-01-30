using CRD.Enums;
using CRD.Models;
using CRD.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace CRD.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {

            this._configuration = configuration;
        }
        public string CreateToken(CreateTokenModel user)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID)
            };

            string symKey = _configuration.GetSection("AppSettings:Token").Value!;


            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            symKey));


            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public User Login(UserLoginRequestDto request)
        {
            string passwordHash = StringUtils.GetHash<SHA256>(request.Password);

            //if (!user.Username.Equals(request.Username))
            //{
            //    return null;
            //}

            if (!request.Password.Equals(passwordHash))
            {
                return null; //BadRequest("User not found.");
            }

            string token = CreateToken(new CreateTokenModel
            {
                Password = passwordHash,
                Username = request.Username,
                UserID = request.IdentityNumber
            });

            return new User { };
        }
    }
}
