namespace LLMConsoleApp.Prompts;

public static class TicketClassifierPrompt
{
    public static string Build(string userInput)
    {
        return $@"
                    You are a support ticket classifier.
                    Classify the message into one category:
                    Billing
                    Technical
                    General

                    Return ONLY valid JSON.

                    {{
                      ""category"": ""Billing|Technical|General"",
                      ""confidence"": number between 0 and 1
                    }}

                    Message:
                    {userInput}
                ";
    }
}