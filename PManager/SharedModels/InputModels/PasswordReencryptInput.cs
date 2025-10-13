using SharedModels.Interfaces;

namespace SharedModels.InputModels
{
    public class PasswordReencryptInput: IApiRoute
    {
        public const string Api = "password/reencrypt";
        static string IApiRoute.Api => Api;

        public string OldKey { get; set; }
        public string NewKey { get; set; }
        public int RecordId { get; set; }
        public int PasswordId { get; set; }
    }
}
