namespace LLMConsoleApp.Models;

using System.Text.Json.Serialization;

public class ClassificationResult
{
    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }
}