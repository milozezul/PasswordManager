using SharedModels.InputModels;

namespace PManagerFrontend.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ResponseWrapper<string>> Login(LoginInput input);
        Task<ResponseWrapper<bool>> Register(LoginInput input);
    }
}