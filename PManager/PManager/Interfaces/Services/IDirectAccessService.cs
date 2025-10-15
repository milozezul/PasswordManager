using SharedModels.APIs.Data.Outputs;
using SharedModels.APIs.DirectAccess.Input;

namespace PManager.Interfaces.Services
{
    public interface IDirectAccessService
    {
        string GetDirectLinkToken(LinkInput input);
        Task<DecryptedPasswordOutput?> GetDirectPassword(string password);
    }
}