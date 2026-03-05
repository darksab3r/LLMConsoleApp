namespace LLMConsoleApp.Core.LLMExecutor.cs;

public class AgentExecutor
{
    private ILLMClient _client;

    public AgentExecutor(ILLMClient client)
    {
        _client = client;
    }

    public async Task<string> RunAsync(string goal)
    {
        var state = goal;
        for (int i = 0; i < 5; i++)
        {
            var prompt = $"Think step-by-step: {state}";
            var response = _client.GenerateAsync(prompt);
            state = response.GetAwaiter().GetResult();
        }
        return state;
    }
}