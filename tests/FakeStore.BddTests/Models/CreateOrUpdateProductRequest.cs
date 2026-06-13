
using System.Text.Json.Serialization;

namespace FakeStore.BddTests.Models;

public sealed class CreateOrUpdateProductRequest
{
    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;

    [JsonPropertyName("price")]
    public decimal Price { get; init; }

    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;

    [JsonPropertyName("image")]
    public string Image { get; init; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; init; } = string.Empty;
}