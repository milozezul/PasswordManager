using SharedModels.Interfaces;

namespace SharedModels.APIs.Auth.Input
{
    public class LoginInput: IPostApiRoute
    {
        public const string Api = "login";
        static string IPostApiRoute.Api => Api;

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
