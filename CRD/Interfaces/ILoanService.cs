namespace CRD.Interfaces
{
    public interface ILoanService
    {
        Task<GenericResponse<Loan>> AddUserLoan(Loan request);
        Task<GenericResponse<List<Loan>>> GetUserLoans(int userID);
        Task<GenericResponseWithoutData> UpdateUserLoan(Loan request);
    }
}
