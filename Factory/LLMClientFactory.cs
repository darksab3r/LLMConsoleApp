using LLMConsoleApp.Capabilities;
using LLMConsoleApp.Providers.Ollama;

namespace LLMConsoleApp.Factory;

public static class LLMClientFactory
{
    public static ILLMClient Create(ModelInfo model, HttpClient httpClient)
    {
        return model.Provider switch
        {
            "ollama" => new OllamaClient(httpClient, model.ModelName),
            _ => throw new NotSupportedException(model.Provider)
        };
    }
}