using SharedModels.Interfaces;

namespace SharedModels.InputModels
{
    public class PasswordDiactivateInput: PasswordStatusInput, IApiRoute
    {
        public const string Api = "password/diactivate";
        static string IApiRoute.Api => Api;
    }
}
