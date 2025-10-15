using SharedModels.Interfaces;

namespace SharedModels.APIs.Data.Inputs
{
    public class PasswordDiactivateInput: PasswordStatus, IPostApiRoute
    {
        public const string Api = "password/diactivate";
        static string IPostApiRoute.Api => Api;
    }
}
