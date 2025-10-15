using SharedModels.Database;
using System.Text.Json.Serialization;

namespace SharedModels.APIs.Data.Outputs
{
    public class RecordPasswordsOutput
    {
        [JsonPropertyName("record")]
        public Record Record { get; set; }
        [JsonPropertyName("passwords")]
        public List<DecryptedPasswordOutput> Passwords { get; set; }
    }
}
