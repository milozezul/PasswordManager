using PManager.Models.Database;
using SharedModels.InputModels;

namespace PManager.Interfaces.Services
{
    public interface IAuthService
    {
        Task<User> CreateUser(LoginInput input);
        Task<string?> Login(LoginInput input);
    }
}