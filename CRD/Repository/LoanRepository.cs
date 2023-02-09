using CRD.Utils;
using Dapper;

namespace CRD.Repository
{
    public class LoanRepository : BaseRepository
    {
        public LoanRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<int> AddUserLoan(AddLoan request, int userID, TransactionWrapper tw)
        {
            var loanID = await tw.Connection.QueryFirstAsync<int>(@"
                                                            declare @ID int;
                                                            INSERT INTO [dbo].[Loan]
                                                               ([UserID]
                                                               ,[LoanType]
                                                               ,[Amount]
                                                               ,[CurrencyCode]
                                                               ,[FromDate]
                                                               ,[ToDate]
                                                               ,[LoanStatusCode])
                                                        VALUES
                                                            (@UserID
                                                            ,@LoanType
                                                            ,@Amount
                                                            ,@CurrencyCode
                                                            ,@FromDate
                                                            ,@ToDate
                                                            ,1
                                                            )
                                                            set @ID = SCOPE_IDENTITY();
                                                            select @ID;
                                                        ", new
            {
                @UserID = userID,
                request.LoanType,
                request.Amount,
                request.CurrencyCode,
                request.FromDate,
                request.ToDate
            }, tw.Transaction);


            return loanID;
        }
        public async Task<List<Loan>> GetUserLoans(int userID, TransactionWrapper tw)
        {
            var loan = await tw.Connection.QueryAsync<Loan>(@"select * from dbo.Loan where UserID = @userID and IsDeleted = 0",
                new
                {
                    @userID = userID,
                });

            return loan.ToList();
        }

        public async Task<Loan> GetLoanById(int loanID)
        {
            var loan = await Connection.QuerySingleOrDefaultAsync<Loan>(@"select * from dbo.Loan where ID = @ID and IsDeleted = 0",
                new
                {
                    ID = loanID,
                });

            return loan;
        }


        public async Task UpdateUserLoan(Loan model, TransactionWrapper tw)
        {

            await tw.Connection.ExecuteAsync(@"
                                            UPDATE [dbo].[Loan]
                                               SET [LoanType] = @LoanType
                                                  ,[Amount] = @Amount
                                                  ,[CurrencyCode] = @CurrencyCode
                                                  ,[FromDate] = @FromDate
                                                  ,[ToDate] = @ToDate
                                                  ,[LoanStatusCode] = @LoanStatusCode
                                                  ,[IsDeleted]=@IsDeleted
                                             WHERE ID = @id 
                                            ",
                new
                {
                    model.LoanType,
                    model.Amount,
                    model.CurrencyCode,
                    model.FromDate,
                    model.ToDate,
                    model.LoanStatusCode,
                    model.IsDeleted,
                    model.ID
                }, transaction: tw.Transaction);

        }

    }
}
