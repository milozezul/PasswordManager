using SharedModels.APIs.Auth.Input;
using SharedModels.APIs.Auth.Output;
using SharedModels.InputModels;

namespace PManagerFrontend.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginInput input);
        Task<ResponseWrapper<bool>> Register(RegisterInput input);
    }
}