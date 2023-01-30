using CRD.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace CRD.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }


        [HttpGet(nameof(GetAllUsers))]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var result = _userService.GetAllUsers();

            return result;
        }

        [HttpGet("{userID}")]
        public async Task<ActionResult<User>> GetUserByID(int userID)
        {
            var result = _userService.GetUserByID(userID);

            if (result is null)
            {
                return NotFound("User Does Not Exist.");
            }

            return result;
        }


        [HttpPost(nameof(Register))]
        public ActionResult<User> Register(UserRequestDto request)
        {

            var result = _userService.Register(request);

            return result;
        }
    }
}
