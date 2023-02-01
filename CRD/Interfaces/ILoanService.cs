namespace CRD.Interfaces
{
    public interface ILoanService
    {
        Task<GenericResponse<Loan>> AddUserLoan(AddLoan request, int userID);
        Task<GenericResponse<List<Loan>>> GetUserLoans(int userID);
        Task<GenericResponseWithoutData> UpdateUserLoan(UpdateLoanRequest request, int userID);
    }
}
