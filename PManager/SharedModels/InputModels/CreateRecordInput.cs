using SharedModels.Interfaces;

namespace SharedModels.InputModels
{
    public class CreateRecordInput: IApiRoute
    {
        public const string Api = "records/create";

        static string IApiRoute.Api => Api;

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
    }
}
