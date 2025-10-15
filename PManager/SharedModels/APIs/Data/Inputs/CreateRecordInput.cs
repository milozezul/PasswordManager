using SharedModels.Interfaces;

namespace SharedModels.APIs.Data.Inputs
{
    public class CreateRecordInput: IPostApiRoute
    {
        public const string Api = "records/create";

        static string IPostApiRoute.Api => Api;

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
    }
}
