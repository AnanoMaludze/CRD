using CRD.Models;

namespace CRD.Interfaces
{
    public interface IUserService
    {
        Task<GenericResponse<User>> GetUserByID(int userID);
        Task<GenericResponse<int>> Register(UserRequestDto request);
        Task<GenericResponse<User>> Login(UserLoginRequestDto request);

    }
}
