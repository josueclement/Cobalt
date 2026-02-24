using Cobalt.Iam.Models;

namespace Cobalt.Iam.Services
{
    public interface IAuthenticationService
    {
        Session? Login(string username, string password);
        void Logout(string token);
        User? ValidateSession(string token);
        string HashPassword(string password);
        User Register(string username, string password, string email);
    }
}
