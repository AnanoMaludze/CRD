using CRD.Enums;
using CRD.Interfaces;
using CRD.Models;
using log4net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRD.Controllers
{
    [Route("api/[controller]")]
    public class LoanController : ApiBaseController
    {

        private readonly ILoanService _loanService;
        private IConfiguration _configuration;
        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(LoanController));
        public LoanController(IConfiguration configuration, ILoanService loanService) : base(configuration)
        {
            this._configuration = configuration;
            this._loanService = loanService;
        }



        [HttpGet(nameof(GetUserLoans))]

        [ProducesResponseType(typeof(GenericResponse<List<Loan>>), 200)]
        public async Task<IActionResult> GetUserLoans(int userID)
        {
            var result = await _loanService.GetUserLoans(userID);

            return JsonContent(result);
        }



        [HttpPut(nameof(UpdateUserLoan))]
        [ProducesResponseType(typeof(GenericResponseWithoutData), 200)]
        public async Task<IActionResult> UpdateUserLoan(Loan request)
        {
            var result = await _loanService.UpdateUserLoan(request);

            return JsonContent(result);
        }

        [HttpPost(nameof(AddUserLoan))]

        [ProducesResponseType(typeof(GenericResponse<Loan>), 200)]
        public async Task<IActionResult> AddUserLoan(Loan request)
        {
            var result = await _loanService.AddUserLoan(request);

            return JsonContent(result);
        }

    }
}