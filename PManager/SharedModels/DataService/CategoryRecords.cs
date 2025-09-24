using SharedModels.Database;
using System.Text.Json.Serialization;

namespace SharedModels.DataService
{
    public class CategoryRecords
    {
        [JsonPropertyName("category")]
        public Category Category { get; set; }
        [JsonPropertyName("records")]
        public List<Record> Records { get; set; }
    }
}
