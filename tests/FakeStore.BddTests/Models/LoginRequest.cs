using System.Text.Json.Serialization;

namespace FakeStore.BddTests.Models
{
    public sealed class LoginRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; init; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; init; } = string.Empty;
    }
}
