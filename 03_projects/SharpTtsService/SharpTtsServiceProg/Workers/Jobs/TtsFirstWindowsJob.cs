using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;

namespace SharpTtsServiceProg.Workers.Jobs
{
    public class TtsFirstWindowsJob : ITtsJob
    {
        private SpeechSynthesizer synth;
        
        private CultureInfo currentCulture;
        private Prompt currentPrompt;

        private bool isInitialized;
        private readonly BuilderJob _builderJob;

        public TtsFirstWindowsJob()
        {
            synth = new SpeechSynthesizer();
                //SetVoiceSettings();
                currentCulture = new CultureInfo("pl-PL");
                SetVoiceSettings2(currentCulture);
                synth.Rate = 0;
                isInitialized = true;
            _builderJob = new BuilderJob();
        }

        private void SetVoiceSettings2(CultureInfo culture)
        {
            string name = culture.Name;
            ReadOnlyCollection<InstalledVoice>? tmp = synth.GetInstalledVoices();
            InstalledVoice? voice = tmp.FirstOrDefault(x => x.VoiceInfo.Culture.Name == name);
            if (voice  != null &&
                currentCulture.Name != name)
            {
                currentCulture = culture;
                synth.SelectVoice(voice.VoiceInfo.Name);
            }
        }

        private void SetVoiceSettings()
        {
            synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0, new CultureInfo("pl-PL"));
        }

        void ITtsJob.SetVoiceSettings2(CultureInfo culture)
        {
            SetVoiceSettings2(culture);
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

        public async Task PlStartNew(object builder)
        {
            await StartNew(builder);
        }

        public async Task EnStartNew(object builder)
        {
            await StartNew(builder);
        }

        public async Task StartNew(
            object builderObj)
        {
            PromptBuilder builder = (PromptBuilder)builderObj;
            if (builder.Culture != null)
            {
                // SetVoiceSettings2(builder.Culture);
            }

            var state = synth.State;
            if (state == SynthesizerState.Speaking ||
                state == SynthesizerState.Paused)
            {
                synth.SpeakAsyncCancel(currentPrompt);
                synth.SpeakAsyncCancelAll();
            }

            //synth.Resume();
            synth.SetOutputToDefaultAudioDevice();
            currentPrompt = synth.SpeakAsync(builder);
        }

        public async Task Stop()
        {
            var state = synth.State;
            if (state == SynthesizerState.Speaking ||
                state == SynthesizerState.Paused)
            {
                synth.SpeakAsyncCancel(currentPrompt);
                synth.SpeakAsyncCancelAll();
            }
        }

        public object GetBuilder(
            (string, string) adrTuple,
            CultureInfo culture)
        {
            var builder = _builderJob.GetBuilder(adrTuple, culture);
            return builder;
        }
        
        //public async Task SaveAudioFile(
        //   string folderName,
        //   string fileName,
        //   string text,
        //   CultureInfo culture = null)
        //{
        //    var builder = CreateBuilder(text);
        //    SaveAudioFile(folderName, fileName, builder);
        //}

        public async Task SaveAudioFile(
            string folderPath,
            string fileName,
            object builder2)
        {
            var builder = (PromptBuilder)builder2;
            if (builder.Culture != null)
            {
                SetVoiceSettings2(builder.Culture);
            }

            var filepath = folderPath + "/" + fileName + ".wav";
            try
            {
                synth.SetOutputToWaveFile(filepath,
                new SpeechAudioFormatInfo(
                    32000,
                    AudioBitsPerSample.Sixteen,
                    AudioChannel.Mono));

                synth.SpeakAsync(builder);
                // Release File - how to ensure?
                //synth.SetOutputToDefaultAudioDevice();
            }
            catch
            {
            }            
        }
    }
}