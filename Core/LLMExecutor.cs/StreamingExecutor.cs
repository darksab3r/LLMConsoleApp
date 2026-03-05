namespace LLMConsoleApp.Core.LLMExecutor.cs;

public class StreamingExecutor
{
    private ILLMClient _client;

    public StreamingExecutor(ILLMClient client)
    {
        _client = client;
    }

    public async IAsyncEnumerable<string> StreamAsync(string prompt)
    {
        await foreach (var token in _client.StreamAsync(prompt))
        {
            yield return token;
        }
    }
}