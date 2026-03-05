using System.Text;
using System.Text.Json;

namespace LLMConsoleApp.Providers.Ollama;

public class OllamaClient : ILLMClient
{
    private const string Endpoint = "/api/generate";

    private readonly HttpClient _httpClient;
    private readonly string _model;

    public OllamaClient(HttpClient httpClient, string model)
    {
        _httpClient = httpClient;
        _model = model;
    }

    public async Task<string> GenerateAsync(string prompt)
    {
        using var response = await SendRequestAsync(prompt, stream: false);

        var raw = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(raw);

        return doc.RootElement
                  .GetProperty("response")
                  .GetString()!;
    }

    public async IAsyncEnumerable<string> StreamAsync(string prompt)
    {
        using var response = await SendRequestAsync(
            prompt,
            stream: true,
            HttpCompletionOption.ResponseHeadersRead);

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line))
                continue;

            using var doc = JsonDocument.Parse(line);

            if (doc.RootElement.TryGetProperty("response", out var token))
            {
                var text = token.GetString();

                if (!string.IsNullOrEmpty(text))
                    yield return text;
            }
        }
    }

    private async Task<HttpResponseMessage> SendRequestAsync(
        string prompt,
        bool stream,
        HttpCompletionOption completion = HttpCompletionOption.ResponseContentRead)
    {
        var payload = JsonSerializer.Serialize(new
        {
            model = _model,
            prompt,
            stream
        });

        using var request = new HttpRequestMessage(HttpMethod.Post, Endpoint)
        {
            Content = new StringContent(payload, Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(request, completion);

        response.EnsureSuccessStatusCode();

        return response;
    }
}