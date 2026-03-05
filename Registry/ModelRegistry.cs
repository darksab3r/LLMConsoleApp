using LLMConsoleApp.Capabilities;

namespace LLMConsoleApp.Registry;

public static class ModelRegistry
{
    private static readonly Dictionary<string, ModelInfo> Models =
        new()
        {
            {
                "local-llama3",
                new ModelInfo
                {
                    Provider = "ollama",
                    ModelName = "llama3",
                    Capabilities = new ModelCapabilities
                    {
                        SupportsStreaming = true,
                        SupportsJsonMode = false
                    }
                }
            }
        };

    public static ModelInfo Get(string key)
    {
        return Models[key];
    }
}