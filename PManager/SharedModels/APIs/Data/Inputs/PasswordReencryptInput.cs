using SharedModels.Interfaces;

namespace SharedModels.APIs.Data.Inputs
{
    public class PasswordReencryptInput: IPostApiRoute
    {
        public const string Api = "password/reencrypt";
        static string IPostApiRoute.Api => Api;

        public string OldKey { get; set; }
        public string NewKey { get; set; }
        public int RecordId { get; set; }
        public int PasswordId { get; set; }
    }
}
