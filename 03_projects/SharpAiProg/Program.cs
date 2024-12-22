using System.Diagnostics;
using Microsoft.Extensions.AI;

namespace SharpAiProg;

class Program
{
    static async Task Main(string[] args)
    {
        string question = "What is your name?";

        Console.WriteLine("Sending query to local Olama...");
        var (localResponse, localDuration) = await AskLocalOlama(question);
        Console.WriteLine($"Local Olama response: {localResponse.Message}");
        Console.WriteLine($"Local Olama duration: {localDuration.TotalMilliseconds} ms\n");

        Console.WriteLine("Sending query to QNAP Olama...");
        var (qnapResponse, qnapDuration) = await AskQnapOlama(question);
        Console.WriteLine($"QNAP Olama response: {qnapResponse.Message}");
        Console.WriteLine($"QNAP Olama duration: {qnapDuration.TotalMilliseconds} ms\n");
    }

    private static async Task<(ChatCompletion Response, TimeSpan Duration)> AskQnapOlama(string question)
    {
        string endpoint = "http://100.117.139.83:32768";
        string modelId = "phi3:mini";
        return await MeasureResponseTime(question, endpoint, modelId);
    }

    private static async Task<(ChatCompletion Response, TimeSpan Duration)> AskLocalOlama(string question)
    {
        string endpoint = "http://localhost:11434/";
        string modelId = "llama3.2:latest";
        return await MeasureResponseTime(question, endpoint, modelId);
    }

    private static async Task<(ChatCompletion Response, TimeSpan Duration)> MeasureResponseTime(
        string question,
        string endpoint,
        string modelId)
    {
        var stopwatch = Stopwatch.StartNew();
        IChatClient chatClient = new OllamaChatClient(new Uri(endpoint), modelId);

        ChatCompletion response = await chatClient.CompleteAsync(question);

        stopwatch.Stop();
        return (response, stopwatch.Elapsed);
    }
}
