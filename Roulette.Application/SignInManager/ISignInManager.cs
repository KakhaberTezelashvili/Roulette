namespace Roulette.Application.SignInManager
{
    public interface ISignInManager
    {
        string GetPassword(string flatPassword, string salt);
        string GenerateToken();
    }
}
