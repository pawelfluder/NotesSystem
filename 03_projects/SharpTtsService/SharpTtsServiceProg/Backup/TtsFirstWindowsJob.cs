// using System.Globalization;
// using System.Speech.AudioFormat;
// using System.Speech.Synthesis;
// using Xamarin.Essentials;
//
// namespace SharpTtsServiceProg.Workers.Jobs;
//
// public class TtsFirstWindowsJob : ITtsJob
// {
//     private SpeechSynthesizer synth;
//         
//     private CultureInfo currentCulture;
//
//     private Prompt currentPrompt;
//
//     private bool isInitialized;
//
//     public TtsFirstWindowsJob()
//     {
//         synth = new SpeechSynthesizer();
//     }
//     
//     public async Task Testing()
//     {
//         await TextToSpeech.SpeakAsync("Hello World");
//         Console.ReadLine();
//     }
//
//     public async Task SpeakNow()
//     {
//
//         var locales = await TextToSpeech.GetLocalesAsync();
//
//         // Grab the first locale
//         var locale = locales.FirstOrDefault();
//
//         var settings = new SpeechOptions()
//         {
//             Volume = .75f,
//             Pitch = 1.0f,
//             Locale = locale
//         };
//
//         await TextToSpeech.SpeakAsync("Hello World", settings);
//     }
//
//     private void SetVoiceSettings2(CultureInfo culture)
//      {
//          var name = culture.Name;
//          var tmp = synth.GetInstalledVoices();
//          var voice = tmp.FirstOrDefault(x => x.VoiceInfo.Culture.Name == name);
//          if (voice  != null &&
//              currentCulture.Name != name)
//          {
//              currentCulture = culture;
//              synth.SelectVoice(voice.VoiceInfo.Name);
//          }
//      }
//
//      private void SetVoiceSettings()
//      {
//          synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0, new CultureInfo("pl-PL"));
//      }
//
//      void ITtsJob.SetVoiceSettings2(CultureInfo culture)
//      {
//          SetVoiceSettings2(culture);
//      }
//
//      public async Task Pause()
//      {
//          synth.Pause();
//      }
//
//      public async Task Resume()
//      {
//          if (synth.State == SynthesizerState.Paused)
//          {
//              synth.Resume();
//              return;
//          }
//      }
//
//      public async Task PlStartNew(object builder)
//      {
//          await StartNew(builder);
//      }
//
//      public async Task EnStartNew(object builder)
//      {
//          await StartNew(builder);
//      }
//
//      public async Task StartNew(
//          object builder)
//      {
//          //if (culture != null)
//          //{
//          //    SetVoiceSettings2(culture);
//          //}
//
//          var state = synth.State;
//          if (state == SynthesizerState.Speaking ||
//              state == SynthesizerState.Paused)
//          {
//              synth.SpeakAsyncCancel(currentPrompt);
//              synth.SpeakAsyncCancelAll();
//          }
//
//          //synth.Resume();
//          synth.SetOutputToDefaultAudioDevice();
//          var builder2 = (PromptBuilder)builder;
//          currentPrompt = synth.SpeakAsync(builder2);
//      }
//
//      public async Task Stop()
//      {
//          var state = synth.State;
//          if (state == SynthesizerState.Speaking ||
//              state == SynthesizerState.Paused)
//          {
//              synth.SpeakAsyncCancel(currentPrompt);
//              synth.SpeakAsyncCancelAll();
//          }
//      }
//
//      public Task SaveAudioFile(string folderPath, string fileName, object builder)
//      {
//          throw new NotImplementedException();
//      }
//
//      public object GetBuilder((string, string) adrTuple, CultureInfo culture)
//      {
//          throw new NotImplementedException();
//      }
//
//      //public async Task SaveAudioFile(
//      //   string folderName,
//      //   string fileName,
//      //   string text,
//      //   CultureInfo culture = null)
//      //{
//      //    var builder = CreateBuilder(text);
//      //    SaveAudioFile(folderName, fileName, builder);
//      //}
//
//      public async Task SaveAudioFile(
//          string folderPath,
//          string fileName,
//          PromptBuilder builder)
//      {
//          if (builder.Culture != null)
//          {
//              SetVoiceSettings2(builder.Culture);
//          }
//
//          var filepath = folderPath + "/" + fileName + ".wav";
//          try
//          {
//              synth.SetOutputToWaveFile(filepath,
//                  new SpeechAudioFormatInfo(
//                      32000,
//                      AudioBitsPerSample.Sixteen,
//                      AudioChannel.Mono));
//
//              synth.SpeakAsync(builder);
//              // Release File - how to ensure?
//              //synth.SetOutputToDefaultAudioDevice();
//          }
//          catch
//          {
//          }            
//      }
// }
