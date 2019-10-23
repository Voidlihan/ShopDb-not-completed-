namespace Shop.Services
{
    public interface ICryptoService
    {
        string EncryptPassword(string password);
        bool VerifyPassword(string passwordCandidate, string password);
    }
}