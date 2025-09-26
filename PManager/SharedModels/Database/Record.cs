using System.Text.Json.Serialization;

namespace SharedModels.Database
{
    public class Record
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("category")]
        public Category Category { get; set; }
    }
}
