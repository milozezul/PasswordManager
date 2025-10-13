using SharedModels.Interfaces;

namespace SharedModels.InputModels
{
    public class PasswordLocationInput: IApiRoute
    {
        public const string Api = "record/password/get";
        static string IApiRoute.Api => Api;

        public int RecordId { get; set; }
        public int PasswordId { get; set; }
        public string Password { get; set; }
    }
}
