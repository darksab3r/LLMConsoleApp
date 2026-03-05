using System.Text.Json;
using LLMConsoleApp.Core.Utils;
using LLMConsoleApp.Models;

namespace LLMConsoleApp.Strategies;

public class TicketClassificationStrategy
    : ITaskStrategy<ClassificationResult>
{
    public string BuildPrompt(string input)
    {
        return $@"
                Classify the support ticket.

                Categories:
                Billing
                Technical
                General

                Return JSON:

                {{
                    ""category"": ""Billing|Technical|General"",
                    ""confidence"": ""number""
                }}
                Ticket:
                {input}
                ";
    }

    public ClassificationResult Parse(string response)
    {
        var json = JsonExtractor.Extract(response);

        return JsonSerializer.Deserialize<ClassificationResult>(json)!;
    }
}