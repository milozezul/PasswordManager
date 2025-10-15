using SharedModels.Interfaces;

namespace SharedModels.APIs.Data.Inputs
{
    public class PasswordActivateInput: PasswordStatus, IPostApiRoute
    {
        public const string Api = "password/activate";
        static string IPostApiRoute.Api => Api;
    }
}
