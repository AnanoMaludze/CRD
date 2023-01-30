using CRD.Enums;
using CRD.Models;

namespace CRD.Services.LoanService
{
    public class LoanService : ILoanService
    {
        public static Loan loan = new Loan();


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

        
        public Loan AddUserLoans(Loan request)
        {
            loan.Amount = request.Amount;
            loan.CurrencyCode = request.CurrencyCode;
            loan.LoanType = request.LoanType;
            loan.LoanStatusCode = LoanStatusCode.Forwarded;
            loan.UserID = request.UserID;
            loan.FromDate = request.FromDate;
            loan.ToDate = request.ToDate;

            return loan;
        }

        public List<Loan> GetUserLoans(int userID)
        {
            var loan = loans.Where(x => x.UserID == userID).ToList();

            if (loan is null)
            {
                return null;
            }

            return loan;
        }

        public Loan UpdateUserLoans(Loan request)
        {

            var loan = loans.Find(x => x.LoanID == request.LoanID &&
            (x.LoanStatusCode != Enums.LoanStatusCode.Forwarded && x.LoanStatusCode != Enums.LoanStatusCode.Accepted));

            if (loan == null)
            {
                return null;
            }

            loan.FromDate = request.FromDate;
            loan.ToDate = request.ToDate;
            loan.CurrencyCode = request.CurrencyCode;
            loan.Amount = request.Amount;
            loan.LoanType = request.LoanType;



            return loan;
        }
    }
}
