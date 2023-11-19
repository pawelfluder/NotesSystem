using SharpTtsServiceProg.Worker;

namespace SharpTtsServiceProg.AAPublic
{
    public interface ITtsService
    {
        TtsWorker Tts { get; }
        RepoTtsWorker RepoTts { get; }
    }
}