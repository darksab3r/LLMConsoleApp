namespace LLMConsoleApp.Core.LLMExecutor.cs;

public class TaskExecutor
{
    private readonly ILLMClient _client;

    public TaskExecutor(ILLMClient client)
    {
        _client = client;
    }

    public async Task<T> ExecuteAsync<T>(
        ITaskStrategy<T> strategy,
        string input)
    {
        var prompt = strategy.BuildPrompt(input);

        var response = await _client.GenerateAsync(prompt);

        return strategy.Parse(response);
    }
}