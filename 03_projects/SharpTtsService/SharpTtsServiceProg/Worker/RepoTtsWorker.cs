using SharpRepoServiceProg.Service;
using SharpTtsServiceProg.AAPublic;
using System.Speech.Synthesis;

namespace SharpTtsServiceProg.Worker
{
    public class RepoTtsWorker
    {
        private readonly TtsWorker ttsWorker;
        private readonly IRepoService repoService;
        private readonly SpeechSynthesizer synth;

        public RepoTtsWorker(IRepoService repoService)
        {
            this.repoService = repoService;
            ttsWorker = new TtsWorker();
        }

        public async Task StartNew((string Repo, string Loca) adrTuple)
        {
            var text = repoService.Methods.GetText3(adrTuple);
            await ttsWorker.StartNew(text);
        }

        public async Task Pause()
        {
            await ttsWorker.Pause();
        }

        public async Task Resume()
        {
            await ttsWorker.Resume();
        }

        public async Task SaveFile((string Repo, string Loca) adrTuple)
        {
            var text = repoService.Methods.GetText3(adrTuple);
            var elemPath = repoService.Methods.GetElemPath(adrTuple);
            var filePath = elemPath + "/" + "lista";

            await ttsWorker.SaveFile(filePath, text);
        }
    }
}
