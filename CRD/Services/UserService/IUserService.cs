namespace CRD.Services.UserService
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User GetUserByID(int userID);
        User Register(UserRequestDto request);
        User Login(UserLoginRequestDto request);
    }
}
