namespace CRD.Interfaces
{
    public interface IUserService
    {
         Task<GenericResponse<User>> GetUserByID(int userID);
         Task<GenericResponse<User>> Register(UserRequestDto request);
         Task<GenericResponse<User>> Login(UserLoginRequestDto request);
    }
}
