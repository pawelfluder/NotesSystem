using Xamarin.Essentials;

public class XamarinEssentialsJob : ITextToSpeechService
{
    public async Task SpeakAsync(string text)
    {
        //await TextToSpeech.Default.SpeakAsync("Hello world!");
        
        var settings = new SpeechOptions { Volume = 0.75f, Pitch = 1.0f };
        await TextToSpeech.SpeakAsync(text, settings);
    }
}

public interface ITextToSpeechService
{
    Task SpeakAsync(string text);
}