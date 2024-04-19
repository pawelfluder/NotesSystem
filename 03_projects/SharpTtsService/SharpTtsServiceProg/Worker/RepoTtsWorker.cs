using SharpFileServiceProg.Service;
using SharpRepoServiceProg.AAPublic;
using SharpVideoServiceProg.AAPublic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Text.RegularExpressions;

namespace SharpTtsServiceProg.Worker
{
    public class RepoTtsWorker : MethodRunner
    {
        private readonly TtsBuilderWorker ttsWorker;
        private readonly IFileService fileService;
        private readonly IRepoService repoService;
        private readonly IVideoService videoService;

        private string fileName;

        public RepoTtsWorker(
            IFileService fileService,
            IRepoService repoService,
            IVideoService videoService)
            : base()
        {
            this.fileService = fileService;
            this.repoService = repoService;
            this.videoService = videoService;
            ttsWorker = new TtsBuilderWorker();
            fileName = "lista";
        }

        public override async Task RunMethodAsync(string methodName, params object[] args)
        {
            if (methodName == "Resume")
            {
                await Resume();
                return;
            }

            if (methodName == "Pause")
            {
                await Pause();
                return;
            }

            if (methodName == "Stop")
            {
                await Stop();
                return;
            }

            var adrTuple = (args[0].ToString(), args[1].ToString());

            if (methodName == "PlStartNew")
            {
                await PlStartNew(adrTuple);
                return;
            }

            if (methodName == "EnStartNew")
            {
                await EnStartNew(adrTuple);
                return;
            }

            if (methodName == "PlSaveAudio")
            {
                await PlSaveAudio(adrTuple);
                return;
            }

            if (methodName == "EnSaveAudio")
            {
                await EnSaveAudio(adrTuple);
                return;
            }

            if (methodName == "VoiceToVideo")
            {
                await VoiceToVideo(adrTuple);
                return;
            }
        }

        public async Task Stop()
        {
            await ttsWorker.Stop();
        }

        public async Task PlStartNew((string Repo, string Loca) adrTuple)
        {
            var culture = new CultureInfo("pl-PL");
            var builder = GetBuilder(adrTuple, culture);
            await ttsWorker.StartNew(builder);
        }

        public async Task EnStartNew((string Repo, string Loca) adrTuple)
        {
            var culture = new CultureInfo("en-GB");
            var builder = GetBuilder(adrTuple, culture);
            await ttsWorker.StartNew(builder);
        }

        public async Task Pause()
        {
            await ttsWorker.Pause();
        }

        public async Task<string> PlSaveAudio((string Repo, string Loca) adrTuple)
        {
            var culture = new CultureInfo("pl-PL");
            var builder = GetBuilder(adrTuple, culture);
            var elemPath = repoService.Methods.GetElemPath(adrTuple);

            await ttsWorker.SaveAudioFile(elemPath, fileName, builder);
            return elemPath;
        }

        public async Task<string> EnSaveAudio((string Repo, string Loca) adrTuple)
        {
            var culture = new CultureInfo("en-GB");
            var builder = GetBuilder(adrTuple, culture);
            var elemPath = repoService.Methods.GetElemPath(adrTuple);

            await ttsWorker.SaveAudioFile(elemPath, fileName, builder);
            return elemPath;
        }

        public async Task VoiceToVideo((string Repo, string Loca) adrTuple)
        {
            var folderPath = repoService.Methods.GetElemPath(adrTuple);
            //var folderPath = await PlSaveAudio(adrTuple);
            var audioFilePath = folderPath + "/" + fileName + ".wav";
            var imageFilePath = "Output/background.png";
            var videoFilePath = folderPath + "/" + fileName + ".mp4";
            await videoService.PosterWithAudio(imageFilePath, audioFilePath, videoFilePath);
        }

        public async Task Resume()
        {
            await ttsWorker.Resume();
        }

        private string GetText((string Repo, string Loca) adrTuple)
        {
            var text = repoService.Methods.GetText3(adrTuple);
            text = text.Replace("//", "");
            return text;
        }

        private PromptBuilder GetBuilder((string Repo, string Loca) adrTuple, CultureInfo culture)
        {
            var text = repoService.Methods.GetText3(adrTuple);
            var elements = fileService.Header.Select2.GetElements(text);
            var builder = new PromptBuilder();
            builder.Culture = culture;

            foreach (var elem in elements)
            {
                if (elem.Type == "Header")
                {
                    HeaderParser(builder, elem.Text);
                }

                if (elem.Type != "Header")
                {
                    LineParser(builder, elem.Text);
                }
            }

            return builder;
        }

        private void HeaderParser(
            PromptBuilder builder,
            string line)
        {
            line = AllReplacements(line);
            builder.AppendText(line);
            builder.AppendBreak();
        }

        private void LineParser(
            PromptBuilder builder,
            string line)
        {
            var version = "v;";
            if (line.StartsWith(version))
            {
                var start = version.Length;
                var stop = line.Length;
                var length = stop - start;
                line = line.Substring(start, length);
            }

            line = AllReplacements(line);
            line.Replace(" m2w ", " man to woman ");

            builder.AppendText(line);
            builder.AppendBreak();
        }

        private string AllReplacements(
            string line)
        {
            line = ReplaceShortcut(line, "m2w", "man to woman");
            return line;
        }


        private string ReplaceShortcut(
            string line,
            string shortcut,
            string replacement)
        {
            var space = ' ';
            if (line.Contains(shortcut))
            {
                var tmp01 = space + shortcut + space;
                if (line.Contains(tmp01))
                {
                    line = line.Replace(tmp01, space + replacement + space);
                }

                var tmp02 = shortcut + space;
                if (line.StartsWith(tmp02))
                {
                    line = line.Replace(tmp02, replacement + space);
                }

                var tmp03 = space + shortcut;
                if (line.EndsWith(tmp03))
                {
                    line = line.Replace(tmp03, space + replacement);
                }
            }

            return line;
        }
    }
}