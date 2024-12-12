using Plugin.TextToSpeech;

namespace SharpTtsServiceProg.Workers.FailedJobs;

public class PluginTextToSpeechJob
{
    // https://www.nuget.org/packages/xam.plugins.texttospeech/
    // https://github.com/jamesmontemagno/TextToSpeechPlugin
    public async Task Testing()
    {
        var isSupported = CrossTextToSpeech.IsSupported;
        await CrossTextToSpeech.Current.Speak("Text to speak");
    }
}