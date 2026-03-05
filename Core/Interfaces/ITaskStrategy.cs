public interface ITaskStrategy<T>
{
    string BuildPrompt(string input);
    T Parse(string response);
}