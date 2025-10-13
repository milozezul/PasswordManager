using SharedModels.Interfaces;

namespace SharedModels.InputModels
{
    public class PasswordAddInput: IApiRoute
    {
        public const string Api = "records/password";

        static string IApiRoute.Api => Api;

        public int RecordId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string NewPassword { get; set; }
        public string Password { get; set; }
    }
}
