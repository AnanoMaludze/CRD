namespace CRD.Services.LoanService
{
    public interface ILoanService
    {
        Loan UpdateUserLoans(Loan request);
        Loan AddUserLoans(Loan request);
        List<Loan> GetUserLoans(int userID);
    }
}
