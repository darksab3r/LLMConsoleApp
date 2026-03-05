public interface ILLMClient
{
    Task<string> GenerateAsync(string prompt);
    IAsyncEnumerable<string> StreamAsync(string prompt);
}