using CRD.Enums;
using CRD.Models;
using CRD.Services.LoanService;
using CRD.Services.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : Controller
    {

        private readonly ILoanService _loanService;
        public LoanController(ILoanService loanService)
        {
            this._loanService = loanService;
        }



        [HttpGet(nameof(GetUserLoans))]
        public async Task<ActionResult<List<Loan>>> GetUserLoans(int userID)
        {
            var result = _loanService.GetUserLoans(userID);

            return Ok(result);
        }



        [HttpPut(nameof(UpdateUserLoans))]
        public async Task<ActionResult<List<Loan>>> UpdateUserLoans(Loan request)
        {
            var result = _loanService.UpdateUserLoans(request);

            return Ok(result);
        }

        [HttpPost(nameof(AddUserLoans))]
        public async Task<ActionResult<Loan>> AddUserLoans(Loan request)
        {
            var result = _loanService.AddUserLoans(request);

            return Ok(result);
        }

    }
}