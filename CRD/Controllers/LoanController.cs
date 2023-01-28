using CRD.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoanController : ControllerBase
    {

        private readonly ILogger<LoanController> _logger;
        private static List<Loan> loans = new List<Loan>
            {
                new Loan
                {
                    UserID = 1,
                    LoanID =1,
                    LoanType = Enums.LoanType.Installment_loan,
                    Amount= 1,
                    LoanStatusCode = Enums.LoanStatusCode.Accepted,
                    CurrencyCode = Enums.Currency.EUR,
                    FromDate = DateTime.Now,
                   ToDate= DateTime.Now.AddMonths(5),
                },
                new Loan
                {
                    UserID = 1,
                    LoanID =2,
                    LoanType = Enums.LoanType.Auto_loan,
                    Amount= 1,
                    LoanStatusCode = Enums.LoanStatusCode.Forwarded,
                    CurrencyCode = Enums.Currency.GEL,
                    FromDate = DateTime.Now,
                   ToDate= DateTime.Now.AddMonths(15),
                },

                new Loan
                {
                    UserID = 2,
                    LoanID =3,
                    LoanType = Enums.LoanType.Auto_loan,
                    Amount= 1,
                    LoanStatusCode = Enums.LoanStatusCode.Rejected,
                    CurrencyCode = Enums.Currency.GEL,
                    FromDate = DateTime.Now,
                   ToDate= DateTime.Now.AddMonths(10),
                },
            };
        public LoanController(ILogger<LoanController> logger)
        {
            _logger = logger;
        }

        [HttpGet(nameof(GetUserLoans))]
        public async Task<ActionResult<List<Loan>>> GetUserLoans(int userID)
        {
            var loan = loans.Find(x => x.UserID == userID);

            if (loan == null)
            {
                return NotFound("Loan does not exist.");
            }

            return Ok(loan);
        }



        [HttpPut()]
        public async Task<ActionResult<List<Loan>>> UpdateUserLoans(Loan request)
        {
            var loan = loans.Find(x => x.UserID == request.UserID && 
            (x.LoanStatusCode != Enums.LoanStatusCode.Forwarded && x.LoanStatusCode != Enums.LoanStatusCode.Accepted));

            if (loan == null)
            {
                return NotFound("Loan does not exist.");
            }

            loan.FromDate = request.FromDate;
            loan.ToDate = request.ToDate;
            loan.CurrencyCode = request.CurrencyCode;
            loan.Amount = request.Amount;
            loan.LoanType = request.LoanType;



            return Ok(loan);
        }

    }
}