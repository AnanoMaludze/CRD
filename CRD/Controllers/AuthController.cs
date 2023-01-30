using CRD.Models;
using CRD.Services.AuthService;
using CRD.Services.UserService;
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


        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }


        [HttpPost(nameof(Login))]
        public ActionResult<User> Login(UserLoginRequestDto request)
        {
            var result = _authService.Login(request);

            return Ok(result);
        }

        private string CreateToken(CreateTokenModel user)
        {
            var result = _authService.CreateToken(user);

            return result;
        }
    }
}
