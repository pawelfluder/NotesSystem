using SharpTtsServiceProg.Worker;

namespace SharpTtsServiceProg.Service
{
    internal class TtsService
    {
        public TtsWorker Tts { get; set; }
        public RepoTtsWorker RepoTts { get; }

        public TtsService()
        {
            Tts = new TtsWorker();
            RepoTts = new RepoTtsWorker();
        }
    }
}
