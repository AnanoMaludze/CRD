using CRD.Enums;
using CRD.Interfaces;
using CRD.Models;
using CRD.Repository;
using CRD.Utils;
using log4net;

namespace CRD.Services
{
    public class LoanService : BaseService, ILoanService
    {
        protected readonly LoanRepository _loanRepository;
        private readonly IConfiguration configuration;

        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(LoanService));
        public LoanService(IConfiguration configuration, LoanRepository loanRepository) : base(configuration)
        {
            _loanRepository = loanRepository;
        }


        public async Task<GenericResponseWithoutData> AddUserLoan(AddLoan request, int userID)
        {
            try
            {
                int insertedLoanID;

                using (var tw = GetTransactionWrapper())
                {
                    insertedLoanID = await _loanRepository.AddUserLoan(request, userID, tw);

                    tw.Transaction.Commit();

                    if (insertedLoanID != 0)
                    {
                        return new GenericResponseWithoutData(status: StatusCode.LOAN_ADDED);
                    }
                    return new GenericResponseWithoutData(status: StatusCode.ERROR);
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }

        }

        public async Task<GenericResponse<List<Loan>>> GetUserLoans(int userID)
        {
            try
            {
                var tw = GetTransactionWrapperWithoutTransaction();

                var userLoans = await _loanRepository.GetUserLoans(userID, tw);

                if (userLoans == null)
                {
                    return new GenericResponse<List<Loan>>(status: StatusCode.USER_DOES_MOT_HAVE_LOAN);
                }
                return new GenericResponse<List<Loan>>(status: StatusCode.SUCCESS, userLoans);
            }
            catch (Exception e)
            {

                log.Error(e);
                throw;
            }

        }

        public async Task<GenericResponseWithoutData> UpdateUserLoan(UpdateLoanRequest request, int userID)
        {
            try
            {
                var loan = await _loanRepository.GetLoanById(request.ID);

                if (loan == null)
                {
                    return new GenericResponseWithoutData(status: StatusCode.LOAN_DOES_NOT_EXIST);
                }

                if (loan.UserID != userID)
                {
                    if (loan.LoanStatusCode == LoanStatusCode.Rejected || loan.LoanStatusCode == LoanStatusCode.Accepted)
                    {
                        return new GenericResponseWithoutData(status: StatusCode.USER_CANNOT_CHANGE_THIS_LOAN);

                    }
                }
                
                if (loan.LoanStatusCode == LoanStatusCode.Rejected || loan.LoanStatusCode == LoanStatusCode.Accepted)
                {
                    return new GenericResponseWithoutData(status: StatusCode.LOAN_CANNOT_BE_UPDATED);

                }
                using (var tw = GetTransactionWrapper())
                {
                    loan.FromDate = request.FromDate;
                    loan.ToDate = request.ToDate;
                    loan.CurrencyCode = request.CurrencyCode;
                    loan.Amount = request.Amount;
                    loan.LoanType = request.LoanType;
                    loan.IsDeleted = request.IsDeleted;

                    await _loanRepository.UpdateUserLoan(loan, tw);

                    tw.Transaction.Commit();

                    return new GenericResponseWithoutData(status: StatusCode.LOAN_UPDATED);

                }

            }
            catch (Exception e)
            {

                log.Error(e);
                throw;
            }
        }
    }
}
