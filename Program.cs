using LLMConsoleApp.Core.LLMExecutor.cs;
using LLMConsoleApp.Factory;
using LLMConsoleApp.Registry;
using LLMConsoleApp.Strategies;

class Program
{
    static async Task Main()
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:11434")
        };

        var model = ModelRegistry.Get("local-llama3");

        var client = LLMClientFactory.Create(model, httpClient);

        var taskExecutor = new TaskExecutor(client);
        var streamingExecutor = new StreamingExecutor(client);
        var agentExecutor = new AgentExecutor(client);

        Console.WriteLine("Modes: task | stream | agent | exit");

        while (true)
        {
            Console.Write("\nmode> ");
            var mode = Console.ReadLine();

            if (mode == "exit")
                break;

            Console.Write("input> ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            switch (mode)
            {
                case "task":
                {
                    var strategy = new TicketClassificationStrategy();

                    var result = await taskExecutor.ExecuteAsync(
                        strategy,
                        input);

                    Console.WriteLine($"Category: {result.Category}");
                    Console.WriteLine($"Confidence: {result.Confidence}");
                    break;
                }

                case "stream":
                {
                    Console.WriteLine("Streaming:");

                    await foreach (var token in streamingExecutor.StreamAsync(input))
                    {
                        Console.Write(token);
                    }

                    Console.WriteLine();
                    break;
                }

                case "agent":
                {
                    Console.WriteLine("Agent reasoning...");

                    var result = await agentExecutor.RunAsync(input);

                    Console.WriteLine($"Agent result: {result}");
                    break;
                }

                default:
                    Console.WriteLine("Unknown mode");
                    break;
            }
        }
    }
}