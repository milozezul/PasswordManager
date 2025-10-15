using PManager.Models.Database;
using SharedModels.APIs.Auth.Input;
using SharedModels.APIs.Auth.Output;

namespace PManager.Interfaces.Services
{
    public interface IAuthService
    {
        Task<User?> CreateUser(RegisterInput input);
        Task<LoginResponse> Login(LoginInput input);
    }
}