using SharpRepoServiceProg.Service;
using SharpTtsServiceProg.AAPublic;
using SharpTtsServiceProg.Worker;

namespace SharpTtsServiceProg.Service
{
    internal class TtsService : ITtsService
    {
        public TtsWorker Tts { get; }
        public RepoTtsWorker RepoTts { get; }

        public TtsService(IRepoService repoService)
        {
            Tts = new TtsWorker();
            RepoTts = new RepoTtsWorker(repoService);
        }
    }
}
