namespace CRD.Interfaces
{
    public interface IAuthService
    {
        string CreateToken(CreateTokenModel user);

    }
}
