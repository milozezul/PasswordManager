using System.Text.Json.Serialization;

namespace SharedModels.APIs.Data.Inputs
{
    public class NoteData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("dateCreated")]
        public DateTime DateCreated { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
