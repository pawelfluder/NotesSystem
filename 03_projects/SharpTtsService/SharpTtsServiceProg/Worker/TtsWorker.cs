using System.Globalization;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using static System.Net.Mime.MediaTypeNames;

namespace SharpTtsServiceProg.Worker
{
    public class TtsWorker
    {
        private SpeechSynthesizer synth;

        public TtsWorker()
        {
            synth = new SpeechSynthesizer();
            //SetVoiceSettings();
            var cultureString = "pl-PL";
            SetVoiceSettings2(cultureString);
        }

        private void SetVoiceSettings2(string cultureString)
        {
            var tmp = synth.GetInstalledVoices();
            var voice = tmp.First(x => x.VoiceInfo.Culture.Name == cultureString);
            synth.SelectVoice(voice.VoiceInfo.Name);
            var gg = synth.Rate;
            synth.Rate = 0;
        }

        private void SetVoiceSettings()
        {
            synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0, new CultureInfo("pl-PL"));
        }

        public async Task Pause()
        {
            synth.Pause();
        }

        public async Task Resume()
        {
            if (synth.State == SynthesizerState.Paused)
            {
                synth.Resume();
                return;
            }
        }

        public async Task StartNew(string text)
        {
            if (synth.State == SynthesizerState.Speaking ||
                synth.State == SynthesizerState.Paused)
            {
                synth.SpeakAsyncCancelAll();
            }
            var state = synth.State;
            synth.Resume();
            synth.SpeakAsync(text);
        }

        public async Task SaveFile(string fileName, string text)
        {
            var filepath2 = fileName + ".wav";
            synth.SetOutputToWaveFile(filepath2,
                new SpeechAudioFormatInfo(
                    32000,
                    AudioBitsPerSample.Sixteen,
                    AudioChannel.Mono));

            //synth.SetOutputToWaveFile(fileName + ".wav");
            synth.SpeakAsync(text);
        }

        public void LoadFile(string text)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();
            synth.Speak("Hello, world!");

            // Changing the Voice
            synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
            synth.Speak("Hello, I am a female voice!");

            // Changing the Pitch and Rate
            synth.Rate = -2;
            synth.Volume = 100;
            synth.Speak("Hello, I am speaking slower and louder!");

            // Pausing and Resuming Speech
            synth.Speak("Hello, I will pause for 3 seconds now.");
            synth.Pause();
            System.Threading.Thread.Sleep(3000); // wait for 3 seconds
            synth.Resume();
            synth.Speak("I am back!");

            // Saving Speech to a WAV File
            synth.SetOutputToWaveFile("output.wav");
            synth.Speak("Hello, I am saving my speech to a WAV file!");

            // Setting the Speech Stream
            MemoryStream stream = new MemoryStream();
            synth.SetOutputToWaveStream(stream);
            synth.Speak("Hello, I am being streamed to a memory stream!");
            byte[] speechBytes = stream.GetBuffer();

            // Changing the Voice and Pronunciation
            PromptBuilder builder = new PromptBuilder();
            builder.StartVoice(VoiceGender.Female, VoiceAge.Adult, 1);
            builder.AppendText("Hello, my name is Emily.");
            builder.StartVoice(VoiceGender.Female, VoiceAge.Teen, 2);
            builder.AppendText("I am from New York City.");
            builder.StartStyle(new PromptStyle() { Emphasis = PromptEmphasis.Strong });
            builder.AppendText("I really love chocolate!");
            builder.EndStyle();
            builder.StartStyle(new PromptStyle() { Emphasis = PromptEmphasis.Reduced });
            builder.AppendText("But I'm allergic to it...");
            builder.EndStyle();
            synth.Speak(builder);

            Console.ReadLine();
        }
    }
}
