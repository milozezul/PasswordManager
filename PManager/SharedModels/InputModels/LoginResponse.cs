using System.Text.Json.Serialization;

namespace SharedModels.InputModels
{
    public class LoginResponse
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
