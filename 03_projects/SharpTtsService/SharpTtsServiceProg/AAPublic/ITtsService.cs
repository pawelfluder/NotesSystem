using SharpTtsServiceProg.Workers.Fasades;
using SharpTtsServiceProg.Workers.Jobs;

namespace SharpTtsServiceProg.AAPublic;

public interface ITtsService
{
    ITtsJob Tts { get; }

    RepoTtsWorker RepoTts { get; }
}
