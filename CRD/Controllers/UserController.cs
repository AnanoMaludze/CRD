using CRD.Interfaces;
using CRD.Models;
using CRD.Services;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CRD.Controllers
{
    public class UserController : ApiBaseController
    {
        private readonly IUserService _userService;


        private readonly IHttpContextAccessor _httpContentAccessor;

        private readonly IAuthService _authService;
        private IConfiguration _configuration;
        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(UserController));
        public UserController(IConfiguration configuration, IUserService userService, IAuthService authService, IHttpContextAccessor httpContentAccessor) 
            : base(configuration, httpContentAccessor)
        {
            this._configuration = configuration;
            this._userService = userService;
            this._authService = authService;
            this._httpContentAccessor = httpContentAccessor;
        }

        [HttpGet(nameof(GetLoggedInUser)), Authorize(Roles = "User")]

        [ProducesResponseType(typeof(GenericResponse<User>), 200)]
        public async Task<IActionResult> GetLoggedInUser()
        {
            var result = await _userService.GetUserByID(GetUserID());

            return JsonContent(result);
        }


        [HttpPost(nameof(Register))]
        [ProducesResponseType(typeof(GenericResponse<int>), 200)]
        public async Task<IActionResult> Register([FromBody] UserRequestDto request)
        {
            
            var result = await _userService.Register(request);

            return JsonContent(result);
        }

        [HttpPost(nameof(Login))]

        [ProducesResponseType(typeof(GenericResponse<string>), 200)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto request)
        {
            var result = await _userService.Login(request);

            if (result.Status == Enums.StatusCode.SUCCESS)
            {
                string token = _authService.CreateToken(new CreateTokenModel
                {
                    UserID = result.Response.ID,
                    Username = result.Response.Username,
                });

                return JsonContent(token);
            }

            return JsonContent(result);
        }

    }
}
