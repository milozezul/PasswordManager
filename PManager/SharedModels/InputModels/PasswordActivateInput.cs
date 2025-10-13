using SharedModels.Interfaces;

namespace SharedModels.InputModels
{
    public class PasswordActivateInput: PasswordStatusInput, IApiRoute
    {
        public const string Api = "password/activate";
        static string IApiRoute.Api => Api;
    }
}
