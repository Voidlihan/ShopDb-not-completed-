using static BCrypt.Net.BCrypt;
using Shop.Services.Interface;
namespace Shop.Services
{
    public class BcryptHasher : ICryptoService
    {
        public string EncryptPassword(string password)
        {
            return HashPassword(password);
        }
        public bool VerifyPassword(string password, string passwordCandidate)
        {
            return Verify(passwordCandidate, password);
        }

    }
}
