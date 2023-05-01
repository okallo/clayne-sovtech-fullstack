using System.Text.Json.Serialization;

namespace clayne_sovtech_fullstack.models;
public class Joke
{
    [JsonPropertyName("id")]
    public string Id { get; set; }= string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; }= string.Empty;
    public string Url { get; set; }= string.Empty;
}