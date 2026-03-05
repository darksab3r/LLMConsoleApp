namespace LLMConsoleApp.Capabilities;

public class ModelCapabilities
{
    public bool SupportsStreaming { get; set; }

    public bool SupportsJsonMode { get; set; }

    public bool SupportsTools { get; set; }

    public bool SupportsVision { get; set; }
}