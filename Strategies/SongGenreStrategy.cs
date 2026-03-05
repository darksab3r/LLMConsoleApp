namespace LLMConsoleApp.Strategies;

public class SongGenreStrategy : ITaskStrategy<string>
{
    public string BuildPrompt(string input)
    {
        return $"""
                Identify the genre of this song.

                Title/Lyrics:
                {input}

                Return only the genre.
                """;
    }

    public string Parse(string response)
    {
        return response.Trim();
    }
}