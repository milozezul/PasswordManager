using SharedModels.APIs.DirectAccess.Input;
using SharedModels.InputModels;

namespace PManagerFrontend.Interfaces.Services
{
    public interface IDirectLinkService
    {
        Task<ResponseWrapper<string>> GetDirectToken(LinkInput input);
    }
}