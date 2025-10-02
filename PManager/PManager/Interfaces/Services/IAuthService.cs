using PManager.Models.Database;
using SharedModels.InputModels;

namespace PManager.Interfaces.Services
{
    public interface IAuthService
    {
        Task<User?> CreateUser(RegisterInput input);
        Task<LoginResponse> Login(LoginInput input);
    }
}