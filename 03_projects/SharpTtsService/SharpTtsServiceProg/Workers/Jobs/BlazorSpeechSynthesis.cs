using System.Globalization;
using BlazorWorker.Extensions.JSRuntime;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using SharpTtsServiceProg.Registration;
using SpeechSynthesisUtterance = Microsoft.JSInterop.SpeechSynthesisUtterance;

namespace SharpTtsServiceProg.Workers.Jobs;

public class BlazorSpeechSynthesis : ITtsJob
{
    private readonly ISpeechSynthesisService _synth;
    // https://www.nuget.org/packages/Blazor.SpeechSynthesis
    // JSInterop implementations:
    // https://www.nuget.org/packages/Tewr.BlazorWorker.Extensions.JSRuntime
    // https://blazorschool.com/tutorial/blazor-server/dotnet7/ijsruntime-783376
    // https://www.nuget.org/packages/Microsoft.JSInterop


    public BlazorSpeechSynthesis()
    {
        _synth = MyBorder.OutContainer.Resolve<ISpeechSynthesisService>();
    }
    public async Task PlStartNew(object builder)
    {
        var voicesArray = await _synth.GetVoicesAsync();
        
        var gg = new SpeechSynthesisUtterance()
        {
            Text = "Hello World",
            Lang = "en-GB",
            Pitch = 1.0,
            Rate = 1.0,
            Voice = voicesArray.First()
        };
        _synth.SpeakAsync(gg);
    }
    public async Task SpeakAsync()
    {
        var builder = WebAssemblyHostBuilder.CreateDefault();
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(
            sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

        builder.Services.AddSpeechSynthesisServices();

        await builder.Build().RunAsync();
        
        var serviceCollection = new ServiceCollection();

        var gg2 = new BlazorWorkerJSRuntime();
        

        serviceCollection.AddSingleton<IJSRuntime, BlazorWorkerJSRuntime>();
        // serviceCollection.AddSingleton<IJSRuntime, JSRuntime>();
        serviceCollection.AddSpeechSynthesisServices();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var speechSynthesisService = serviceProvider.GetRequiredService<ISpeechSynthesisService>();

        var voicesArray = await speechSynthesisService.GetVoicesAsync();
        
        var gg = new SpeechSynthesisUtterance()
        {
            Text = "Hello World",
            Lang = "en-GB",
            Pitch = 1.0,
            Rate = 1.0,
            Voice = voicesArray.First()
        };
        speechSynthesisService.SpeakAsync(gg);
        
        // var type1 = typeof(Microsoft.JSInterop.ISpeechSynthesisService);
        // var assembly = Assembly.GetAssembly(type1);
        // var type2 = assembly
        //     .GetType("Microsoft.JSInterop.SpeechSynthesisService");
        //
        // IJSRuntime jsRuntime = new JSRuntime();
        // var instance = Activator.CreateInstance(type2);

    }
    
    public class JSRuntimeImplementation : IJSRuntime
    {
        public ValueTask<T> InvokeAsync<T>(
            string identifier,
            params object[] args)
        {
            // Implementacja wywołania JavaScript w tym przypadku, np. mock
            Console.WriteLine($"Invoking JS function: {identifier}");
            return new ValueTask<T>();
        }

        public ValueTask<TValue> InvokeAsync<TValue>(
            string identifier,
            CancellationToken cancellationToken,
            object?[]? args)
        {
            // Implementacja wywołania JavaScript w tym przypadku, np. mock
            Console.WriteLine($"Invoking JS function: {identifier}");
            return new ValueTask<TValue>();
        }

        
    }

    public void SetVoiceSettings2(CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public Task Pause()
    {
        throw new NotImplementedException();
    }

    public Task Resume()
    {
        throw new NotImplementedException();
    }

   

    public Task EnStartNew(object builder)
    {
        throw new NotImplementedException();
    }

    public Task Stop()
    {
        throw new NotImplementedException();
    }

    public Task SaveAudioFile(string folderPath, string fileName, object builder)
    {
        throw new NotImplementedException();
    }

    public object GetBuilder((string, string) adrTuple, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class App : IComponent
{
    public void Attach(RenderHandle renderHandle)
    {
        throw new NotImplementedException();
    }

    public Task SetParametersAsync(ParameterView parameters)
    {
        throw new NotImplementedException();
    }
}