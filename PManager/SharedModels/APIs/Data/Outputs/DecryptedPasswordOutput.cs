using SharedModels.APIs.Data.Inputs;
using System.Text.Json.Serialization;

namespace SharedModels.APIs.Data.Outputs
{
    public class DecryptedPasswordOutput
    {
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
        [JsonPropertyName("dateCreated")]
        public DateTime DateCreated { get; set; }
        [JsonPropertyName("expirationDate")]
        public DateTime? ExpirationDate { get; set; }
        [JsonPropertyName("notes")]
        public List<NoteData> Notes { get; set; }

    }
}
