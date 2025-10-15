using SharedModels.Interfaces;

namespace SharedModels.APIs.Data.Inputs
{
    public class PasswordAddInput: IPostApiRoute
    {
        public const string Api = "records/password";

        static string IPostApiRoute.Api => Api;

        public int RecordId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string NewPassword { get; set; }
        public string Password { get; set; }
    }
}
