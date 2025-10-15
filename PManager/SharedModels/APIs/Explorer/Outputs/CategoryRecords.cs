using SharedModels.Database;
using SharedModels.Interfaces;
using System.Text.Json.Serialization;

namespace SharedModels.APIs.Explorer.Outputs
{
    public class CategoryRecords : IGetApiRoute
    {
        public const string Api = "records";
        static string IGetApiRoute.Api => Api;

        [JsonPropertyName("category")]
        public Category Category { get; set; }
        [JsonPropertyName("records")]
        public List<Record> Records { get; set; }
    }
}
