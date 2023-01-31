using CRD.Enums;
using CRD.Interfaces;
using CRD.Models;
using CRD.Repository;
using CRD.Utils;

namespace CRD.Services
{
    public class LoanService : BaseService, ILoanService
    {
        protected readonly LoanRepository _loanRepository;
        private readonly IConfiguration configuration;

        public LoanService(IConfiguration configuration, LoanRepository loanRepository) : base(configuration)
        {
            _loanRepository = loanRepository;
        }


        public async Task<GenericResponse<Loan>> AddUserLoan(Loan request)
        {
            try
            {
                using (var tw = GetTransactionWrapper())
                {
                    var insertedLoanID = await _loanRepository.AddUserLoan(request, tw);

                    var loan = await _loanRepository.GetLoanById(insertedLoanID, tw);

                    if (loan == null)
                    {
                        return new GenericResponse<Loan>(status: StatusCode.ERROR);

                    }

                    return new GenericResponse<Loan>(status: StatusCode.LOAN_ADDED, loan);
                }
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public async Task<GenericResponse<List<Loan>>> GetUserLoans(int userID)
        {
            try
            {
                var tw = GetTransactionWrapperWithoutTransaction();

                var userLoans = await _loanRepository.GetUserLoans(userID);

                if (userLoans == null)
                {
                    return new GenericResponse<List<Loan>>(status: StatusCode.USER_DOES_MOT_HAVE_LOAN);
                }
                return new GenericResponse<List<Loan>>(status: StatusCode.SUCCESS, userLoans);
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public async Task<GenericResponseWithoutData> UpdateUserLoan(Loan request)
        {
            try
            {
                using (var tw = GetTransactionWrapper())
                {

                    var loan = await _loanRepository.GetLoanById(request.ID, tw);

                    if (loan == null)
                    {
                        return new GenericResponseWithoutData(status: StatusCode.ERROR);

                    }
                    if (loan.LoanStatusCode == LoanStatusCode.Rejected || loan.LoanStatusCode == LoanStatusCode.Accepted)
                    {
                        return new GenericResponseWithoutData(status: StatusCode.LOAN_CANNOT_BE_UPDATED);

                    }
                    loan.FromDate = request.FromDate;
                    loan.ToDate = request.ToDate;
                    loan.CurrencyCode = request.CurrencyCode;
                    loan.Amount = request.Amount;
                    loan.LoanType = request.LoanType;

                    await _loanRepository.UpdateUserLoan(loan, tw);

                    return new GenericResponseWithoutData(status: StatusCode.LOAN_UPDATED);

                }

            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
