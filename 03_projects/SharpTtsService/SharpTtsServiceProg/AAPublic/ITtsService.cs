using SharpTtsServiceProg.Worker;
using SharpTtsServiceProg.Workers.Jobs;

namespace SharpTtsServiceProg.AAPublic;

public interface ITtsService
{
    TtsBuilderWorker Tts { get; }

    RepoTtsWorker RepoTts { get; }
}