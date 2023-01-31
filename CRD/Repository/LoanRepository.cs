using CRD.Utils;
using Dapper;

namespace CRD.Repository
{
    public class LoanRepository : BaseRepository
    {
        public LoanRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<int> AddUserLoan(Loan request, TransactionWrapper tw)
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
                                                            ,@LoanStatusCode
                                                            )
                                                            set @ID = SCOPE_IDENTITY();
                                                            select @ID;
                                                        ", new
            {
                

            });


            return loanID;
        }
        public async Task<List<Loan>> GetUserLoans(int userID)
        {
            var loan = await Connection.QueryAsync<Loan>(@"select * from dbo.Loan where UserID = @userID",
                new
                {
                    ID = userID,
                });

            return loan.ToList(); 
        }

        public async Task<Loan> GetLoanById(int loanID, TransactionWrapper tw)
        {
            var loan = await Connection.QuerySingleOrDefaultAsync<Loan>(@"select * from dbo.Loan where ID = @ID",
                new
                {
                    ID = loanID,
                }, transaction: tw.Transaction);

            return loan;
        }


        public async Task UpdateUserLoan(Loan model, TransactionWrapper tw)
        {

            await Connection.ExecuteAsync(@"
                                            UPDATE [dbo].[Loan]
                                               SET [LoanType] = @LoanType
                                                  ,[Amount] = @Amount
                                                  ,[CurrencyCode] = @CurrencyCode
                                                  ,[FromDate] = @FromDate
                                                  ,[ToDate] = @ToDate
                                                  ,[LoanStatusCode] = @LoanStatusCode
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
                    model.ID
                }, transaction: tw.Transaction);
        }

    }
}
