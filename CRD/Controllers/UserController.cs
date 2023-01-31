using CRD.Interfaces;
using CRD.Models;
using CRD.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace CRD.Controllers
{
    public class UserController : ApiBaseController
    {
        private readonly IUserService _userService;
        private IConfiguration _configuration;
        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(UserController));
        public UserController(IConfiguration configuration, IUserService userService) : base(configuration)
        {
            this._configuration = configuration;
            this._userService = userService;
        }

        [HttpGet("{userID}")]
        [ProducesResponseType(typeof(GenericResponse<User>), 200)]
        public async Task<IActionResult> GetUserByID(int userID)
        {
            var result = await _userService.GetUserByID(userID);

            return JsonContent(result);
        }


        [HttpPost(nameof(Register))]
        [ProducesResponseType(typeof(GenericResponse<User>), 200)]
        public async Task<IActionResult> Register(UserRequestDto request)
        {

            var result = await _userService.Register(request);

            return JsonContent(result);
        }

        [HttpPost(nameof(Login))]

        [ProducesResponseType(typeof(GenericResponse<User>), 200)]
        public async Task<IActionResult> Login(UserLoginRequestDto request)
        {
            var result = await _userService.Login(request);

            return JsonContent(result);
        }

    }
}
