using CRD.Enums;
using CRD.Interfaces;
using CRD.Models;
using CRD.Services;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRD.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class LoanController : ApiBaseController
    {
        private readonly ILoanService _loanService;
        protected readonly IUserService _userService;
        private IConfiguration _configuration;
        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(LoanController));
        public LoanController(IConfiguration configuration, ILoanService loanService, IUserService userService) : base(configuration)
        {
            this._configuration = configuration;
            this._loanService = loanService;
             this._userService = userService;
        }

        [HttpGet("GetUserLoans"), Authorize(Roles = "User")]

        [ProducesResponseType(typeof(GenericResponse<List<Loan>>), 200)]
        public async Task<IActionResult> GetUserLoans()
        {

            int useridint = 0;

            (int.TryParse)(_userService.GetUserID(), out useridint);

            var result = await _loanService.GetUserLoans(useridint);

            return JsonContent(result);
        }


        [HttpPut("UpdateUserLoan"), Authorize(Roles = "User")]

        [ProducesResponseType(typeof(GenericResponseWithoutData), 200)]
        public async Task<IActionResult> UpdateUserLoan(UpdateLoanRequest request)
        {
            HttpContext.Request.EnableBuffering();
            int useridint = 0;

            (int.TryParse)(_userService.GetUserID(), out useridint);

            var result = await _loanService.UpdateUserLoan(request, useridint);

            return JsonContent(result);
        }

        [HttpPost("AddUserLoan"), Authorize(Roles = "User")]

        [ProducesResponseType(typeof(GenericResponse<Loan>), 200)]
        public async Task<IActionResult> AddUserLoan(AddLoan request)
        {
            int userID = 0;

            (int.TryParse)(_userService.GetUserID(), out userID);

            var result = await _loanService.AddUserLoan(request, userID);

            return JsonContent(result);
        }

    }
}