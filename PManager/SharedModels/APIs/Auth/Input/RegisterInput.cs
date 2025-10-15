using SharedModels.Interfaces;
using System.Text.Json.Serialization;

namespace SharedModels.APIs.Auth.Input
{
    public class RegisterInput: IPostApiRoute
    {
        public const string Api = "register";
        static string IPostApiRoute.Api => Api;

        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
