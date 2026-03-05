namespace LLMConsoleApp.Core.Utils;

public class JsonExtractor
{
    public static string Extract(string json)
    {
        var start = json.IndexOf('{');
        var end = json.LastIndexOf('}');

        if (start < 0 || start > end)
            throw new Exception("JSON not found");
        
        return json.Substring(start, end - start+1);
    }
}