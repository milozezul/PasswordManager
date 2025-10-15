using SharedModels.Interfaces;

namespace SharedModels.APIs.Data.Inputs
{
    public class PasswordDataInput: IPostApiRoute
    {
        public const string Api = "record/password/get";
        static string IPostApiRoute.Api => Api;

        public int RecordId { get; set; }
        public int PasswordId { get; set; }
        public string Password { get; set; }
    }
}
