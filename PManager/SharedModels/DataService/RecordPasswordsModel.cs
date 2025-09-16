using SharedModels.Database;
using System.Text.Json.Serialization;

namespace SharedModels.DataService
{
    public class RecordPasswordsModel
    {
        [JsonPropertyName("record")]
        public Record Record { get; set; }
        [JsonPropertyName("passwords")]
        public List<DecryptedPassword> Passwords { get; set; }
    }
}
