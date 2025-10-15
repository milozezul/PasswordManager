using System.Text.Json.Serialization;

namespace SharedModels.InputModels
{
    public class ResponseWrapper<T>
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonPropertyName("value")]
        public T Value { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
