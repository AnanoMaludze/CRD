using CRD.Models;
using CRD.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace CRD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        private static List<User> users = new List<User>
            {
                new User
                { UserId = 1,
                    Name = "name",
                    Surname = "surname",
                    IdentityNumber = "0101010101",
                    Username= "username",
                    BirthDate = DateTime.Now,
                    PasswordHash = "asdasd"
                },
                 new User
                { UserId = 2,
                    Name = "iron man",
                    Surname = "secondSurname",
                    IdentityNumber = "01010121201",
                    Username= "secondusername",
                    BirthDate = DateTime.Now,
                    PasswordHash = "blablabla"
                },
            };


        [HttpGet(nameof(GetAllUsers))]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return Ok(users);
        }

        [HttpGet("{userID}")]
        public async Task<ActionResult<User>> GetUserByID(int userID)
        {
            var user = users.Find(x => x.UserId == userID);

            if (user == null)
            {
                return NotFound("User does not exist.");
            }

            return Ok(user);
        }


        [HttpPost(nameof(Register))]
        public ActionResult<User> Register(UserRequestDto request)
        {

            string passwordHash = StringUtils.GetHash<SHA256>(request.Password);

            user.Name = request.Name;
            user.Surname = request.Surname;
            user.IdentityNumber = request.IdentityNumber;
            user.Username = request.Username;
            user.BirthDate = request.BirthDate;
            user.PasswordHash = passwordHash;

            users.Add(user);

            return Ok(user);
        }

        [HttpPost(nameof(Login))]
        public ActionResult<User> Login(UserLoginRequestDto request)
        {

            string passwordHash = StringUtils.GetHash<SHA256>(request.Password);

            if (!user.Username.Equals(request.Username))
            {
                return BadRequest("User not found.");
            }

            if (!request.Password.Equals(passwordHash))
            {
                return BadRequest("User not found.");
            }

            string token = CreateToken(new CreateTokenModel
            {
                Password = passwordHash,
                Username = request.Username,
                UserID = request.IdentityNumber
            });

            return Ok(user);
        }

        private string CreateToken(CreateTokenModel user)
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
    }
}
