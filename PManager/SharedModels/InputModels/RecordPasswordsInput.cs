using SharedModels.Interfaces;

namespace SharedModels.InputModels
{
    public class RecordPasswordsInput: IApiRoute
    {
        public const string Api = "records";
        static string IApiRoute.Api => Api;

        public int RecordId { get; set; }
        public string Password { get; set; }
    }
}
