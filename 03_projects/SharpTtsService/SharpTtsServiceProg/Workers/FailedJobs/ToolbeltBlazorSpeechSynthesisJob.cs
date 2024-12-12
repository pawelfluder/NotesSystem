using System.Globalization;
using BlazorWorker.Extensions.JSRuntime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using SharpTtsServiceProg.Registration;
using SharpTtsServiceProg.Workers.Jobs;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Toolbelt.Blazor.SpeechSynthesis;
using SpeechSynthesisUtterance = Toolbelt.Blazor.SpeechSynthesis.SpeechSynthesisUtterance;

namespace SharpTtsServiceProg.Workers.FailedJobs;

public class ToolbeltBlazorSpeechSynthesisJob : ITtsJob
{
    private readonly SpeechSynthesis _synth;
    // https://blazorhelpwebsite.com/filedownloads
    
    // https://github.com/jsakamoto/Toolbelt.Blazor.SpeechSynthesis
    // live demo example:
    // https://jsakamoto.github.io/Toolbelt.Blazor.SpeechSynthesis
    // JSInterop implementations:
    // https://www.nuget.org/packages/Tewr.BlazorWorker.Extensions.JSRuntime
    // https://blazorschool.com/tutorial/blazor-server/dotnet7/ijsruntime-783376
    // https://www.nuget.org/packages/Microsoft.JSInterop

    public ToolbeltBlazorSpeechSynthesisJob()
    {
        _synth = MyBorder.OutContainer.Resolve<SpeechSynthesis>();
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
        _synth.Speak(gg);
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
