using SharpTtsServiceProg.Workers.Jobs;

namespace SharpTtsServiceProg;

class Program
{
    static async Task Main(string[] args)
    {
        BlazorSpeechSynthesis job4 = new BlazorSpeechSynthesis();
        await job4.SpeakAsync();
        
        PluginTextToSpeechJob job3 = new();
        job3.Testing();
        
        SpeechSynthesisServiceJob job2 = new ();
        await job2.SpeakAsync();
        
        var job1 = new PluginTextToSpeechJob();
        await job1.Testing();
        
        
        //var reg = new SharpSetupProg21Private.AAPublic.Registration().Start();

        //var text = "Witam serdecznie wszystkich uczestników!";
        //var fileName = "output";
        //ttsService.Tts.Speak(text);

        // var ttsService = new TtsService();
        // var adrTuple = ("Rama", "03/02/16/01");
        // ttsService.RepoTts.Speak(adrTuple);
        Console.ReadLine();
    }
}