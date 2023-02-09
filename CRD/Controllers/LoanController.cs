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


        private readonly IHttpContextAccessor _httpContentAccessor;

        protected readonly IUserService _userService;
        private IConfiguration _configuration;
        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(LoanController));
        public LoanController(IConfiguration configuration, ILoanService loanService, IUserService userService, IHttpContextAccessor httpContentAccessor)
            : base(configuration, httpContentAccessor)
        {
            this._configuration = configuration;
            this._loanService = loanService;
            this._userService = userService;
            this._httpContentAccessor = httpContentAccessor;
        }

        [HttpGet(nameof(GetUserLoans)), Authorize(Roles = "User")]

        [ProducesResponseType(typeof(GenericResponse<List<Loan>>), 200)]
        public async Task<IActionResult> GetUserLoans()
        {

            var result = await _loanService.GetUserLoans(GetUserID());

            return JsonContent(result);
        }


        [HttpPut(nameof(UpdateUserLoan)), Authorize(Roles = "User")]

        [ProducesResponseType(typeof(GenericResponseWithoutData), 200)]
        public async Task<IActionResult> UpdateUserLoan([FromBody] UpdateLoanRequest request)
        {
            var result = await _loanService.UpdateUserLoan(request, GetUserID());

            return JsonContent(result);
        } 

        [HttpPost(nameof(AddUserLoan)), Authorize(Roles = "User")]

        [ProducesResponseType(typeof(GenericResponseWithoutData), 200)]
        public async Task<IActionResult> AddUserLoan([FromBody] AddLoan request)
        {

            var result = await _loanService.AddUserLoan(request, GetUserID());

            return JsonContent(result);
        }

    }
}