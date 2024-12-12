using SharpOperationsProg.AAPublic.Operations;
using SharpRepoServiceProg.AAPublic;
using SharpTtsServiceProg.AAPublic;
using SharpTtsServiceProg.Workers.FailedJobs;
using SharpTtsServiceProg.Workers.Fasades;
using SharpTtsServiceProg.Workers.Jobs;
using SharpVideoServiceProg.AAPublic;

namespace SharpTtsServiceProg.Services;

internal class TtsService : ITtsService
{
    private IOperationsService _operationsService;
    private IRepoService _repoService;
    private IVideoService _videoService;
    private Lazy<RepoTtsWorker> _repoTts;
    private Lazy<ITtsJob> _tts;
    public RepoTtsWorker RepoTts => _repoTts.Value;
    public ITtsJob Tts => _tts.Value;
    public TtsService(
        IOperationsService operationsService,
        IRepoService repoService,
        IVideoService videoService)
    {
        _operationsService = operationsService;
        _repoService = repoService;
        _videoService = videoService;
        _tts = new Lazy<ITtsJob>(
            () => new BlazorSpeechSynthesis());
        _repoTts = new Lazy<RepoTtsWorker>(
            () => new RepoTtsWorker(repoService, videoService, _tts.Value));
    }
}
