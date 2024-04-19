using SharpTtsServiceProg.Worker;

namespace SharpTtsServiceProg.AAPublic
{
    public interface ITtsService
    {
        TtsBuilderWorker Tts { get; }

        RepoTtsWorker RepoTts { get; }
    }
}