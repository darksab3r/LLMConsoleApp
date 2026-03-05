namespace LLMConsoleApp.Capabilities;

public class ModelInfo
{
    public string Provider { get; set; } = "";

    public string ModelName { get; set; } = "";

    public ModelCapabilities Capabilities { get; set; } = new();
}