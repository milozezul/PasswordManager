using PManager.Models.Database;

namespace PManager.Interfaces.Services
{
    public interface IAuthService
    {
        Task<User> CreateUser(string username, string password);
        Task<string?> Login(string username, string password);
    }
}