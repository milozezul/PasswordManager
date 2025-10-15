using SharedModels.Interfaces;

namespace SharedModels.APIs.Data.Inputs
{
    public class RecordPasswordsInput: IPostApiRoute
    {
        public const string Api = "records";
        static string IPostApiRoute.Api => Api;

        public int RecordId { get; set; }
        public string Password { get; set; }
    }
}
