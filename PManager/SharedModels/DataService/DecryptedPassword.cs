using System.Text.Json.Serialization;

namespace SharedModels.DataService
{
    public class DecryptedPassword
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
