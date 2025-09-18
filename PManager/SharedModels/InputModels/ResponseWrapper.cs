using System.Text.Json.Serialization;

namespace SharedModels.InputModels
{
    public class ResponseWrapper<T>
    {
        [JsonPropertyName("value")]
        public T Value { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
