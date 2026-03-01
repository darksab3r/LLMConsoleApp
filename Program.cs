using System.Text;
using System.Text.Json;

class Program
{
    static async Task Main()
    {
        using var httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:11434")
        };

        Console.WriteLine("Local LLM (Ollama). Type 'exit' to quit.");

        while (true)
        {
            Console.Write("\nYou: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                break;

            await CallOllama(httpClient, input);
        }
    }

    static async Task<string> CallOllama(HttpClient httpClient, string userInput)
    {
        var requestBody = new
        {
            model = "llama3",
            prompt = $@"
                        You are a support ticket classifier.
                        Classify the user message into one of:
                        Billing, Technical, General.
                        Return ONLY valid JSON.
                        Do NOT add explanation.
                        Do NOT add text before or after.
                        Output format:
                        {{
                          ""category"": ""Billing|Technical|General"",
                          ""confidence"": number between 0 and 1
                        }}
                        User message:
                        {userInput}
                    ",
            stream = true
        };

        var json = JsonSerializer.Serialize(requestBody);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/generate")
        {
            Content = content
        };

        var response = await httpClient.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead
        );

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return $"Error: {response.StatusCode}\n{error}";
        }

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        var finalResponse = new StringBuilder();

        Console.Write("AI: ");

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line))
                continue;

            try
            {
                using var doc = JsonDocument.Parse(line);

                var root = doc.RootElement;

                if (root.TryGetProperty("response", out var token))
                {
                    var text = token.GetString();
                    if (!string.IsNullOrEmpty(text))
                    {
                        //Console.Write(text); // stream to console
                        finalResponse.Append(text);
                    }
                }

                // stop when done
                if (root.TryGetProperty("done", out var done) && done.GetBoolean())
                {
                    break;
                }
            }
            catch
            {
                // ignore malformed partial lines
            }
        }

        Console.WriteLine();
        return finalResponse.ToString();
    }

    static string ExtractJson(string text)
    {
        var start = text.IndexOf('{');
        var end = text.IndexOf('}');
        if (start >= 0 && end > start)
            return text.Substring(start, end - start + 1);
        throw new Exception("Invalid JSON format");
    }
    
}