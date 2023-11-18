using SharpRepoServiceProg.Service;
using SharpTtsServiceProg.AAPublic;
using System.Speech.Synthesis;

namespace SharpTtsServiceProg.Worker
{
    internal class RepoTtsWorker
    {
        private readonly TtsWorker ttsWorker;
        private readonly IRepoService repoService;
        private readonly SpeechSynthesizer synth;

        public RepoTtsWorker()
        {
            repoService = MyBorder.Container.Resolve<IRepoService>();
            ttsWorker = new TtsWorker();
        }

        public void Speak((string Repo, string Loca) adrTuple)
        {
            var text = repoService.Methods.GetText3(adrTuple);
            ttsWorker.Speak(text);
        }

        public void SaveFile((string Repo, string Loca) adrTuple)
        {
            var text = repoService.Methods.GetText3(adrTuple);
            ttsWorker.Speak(text);
        }
    }
}
