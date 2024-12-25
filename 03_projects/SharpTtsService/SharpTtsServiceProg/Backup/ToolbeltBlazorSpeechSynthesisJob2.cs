// using BlazorWorker.Extensions.JSRuntime;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.JSInterop;
// using Toolbelt.Blazor.Extensions.DependencyInjection;
// using Toolbelt.Blazor.SpeechSynthesis;
// using SpeechSynthesisUtterance = Toolbelt.Blazor.SpeechSynthesis.SpeechSynthesisUtterance;
//
// namespace SharpTtsServiceProg.Workers.FailedJobs;
//
// public class ToolbeltBlazorSpeechSynthesisJob2
// {
//     // https://github.com/jsakamoto/Toolbelt.Blazor.SpeechSynthesis
//     // live demo example:
//     // https://jsakamoto.github.io/Toolbelt.Blazor.SpeechSynthesis
//     // JSInterop implementations:
//     // https://www.nuget.org/packages/Tewr.BlazorWorker.Extensions.JSRuntime
//     // https://blazorschool.com/tutorial/blazor-server/dotnet7/ijsruntime-783376
//     // https://www.nuget.org/packages/Microsoft.JSInterop
//     
//     public async Task SpeakAsync()
//     {
//         var serviceCollection = new ServiceCollection();
//
//         var gg2 = new BlazorWorkerJSRuntime();
//         
//
//         // serviceCollection.AddSingleton<IJSRuntime, BlazorWorkerJSRuntime>();
//         serviceCollection.AddSingleton<IJSRuntime, JSRuntime>();
//         serviceCollection.AddSpeechSynthesis();
//         var serviceProvider = serviceCollection.BuildServiceProvider();
//         
//         var speechSynthesisService = serviceProvider.GetRequiredService<SpeechSynthesis>();
//
//         var voicesArray = await speechSynthesisService.GetVoicesAsync();
//         
//         var gg = new SpeechSynthesisUtterance()
//         {
//             Text = "Hello World",
//             Lang = "en-GB",
//             Pitch = 1.0,
//             Rate = 1.0,
//             Voice = voicesArray.First()
//         };
//         speechSynthesisService.Speak(gg);
//         
//         // var type1 = typeof(Microsoft.JSInterop.ISpeechSynthesisService);
//         // var assembly = Assembly.GetAssembly(type1);
//         // var type2 = assembly
//         //     .GetType("Microsoft.JSInterop.SpeechSynthesisService");
//         //
//         // IJSRuntime jsRuntime = new JSRuntime();
//         // var instance = Activator.CreateInstance(type2);
//
//     }
//     
//     public class JSRuntimeImplementation : IJSRuntime
//     {
//         public ValueTask<T> InvokeAsync<T>(
//             string identifier,
//             params object[] args)
//         {
//             // Implementacja wywołania JavaScript w tym przypadku, np. mock
//             Console.WriteLine($"Invoking JS function: {identifier}");
//             return new ValueTask<T>();
//         }
//
//         public ValueTask<TValue> InvokeAsync<TValue>(
//             string identifier,
//             CancellationToken cancellationToken,
//             object?[]? args)
//         {
//             // Implementacja wywołania JavaScript w tym przypadku, np. mock
//             Console.WriteLine($"Invoking JS function: {identifier}");
//             return new ValueTask<TValue>();
//         }
//
//         
//     }
// }