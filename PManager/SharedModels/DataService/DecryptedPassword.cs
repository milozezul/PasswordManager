using SharedModels.Database;
using System.Text.Json.Serialization;

namespace SharedModels.DataService
{
    public class DecryptedPassword
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
        public List<Note> Notes { get; set; }

    }
}
