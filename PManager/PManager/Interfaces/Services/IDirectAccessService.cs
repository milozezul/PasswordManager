using SharedModels.DataService;
using SharedModels.InputModels;

namespace PManager.Interfaces.Services
{
    public interface IDirectAccessService
    {
        string GetDirectLinkToken(LinkInput input);
        Task<DecryptedPassword?> GetDirectPassword(string password);
    }
}