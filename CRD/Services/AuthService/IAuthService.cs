namespace CRD.Services.AuthService
{
    public interface IAuthService
    {
        string CreateToken(CreateTokenModel user);
        User Login(UserLoginRequestDto request);

    }
}
