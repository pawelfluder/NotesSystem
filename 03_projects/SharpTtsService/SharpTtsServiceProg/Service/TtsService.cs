using SharpOperationsProg.AAPublic.Operations;
using SharpRepoServiceProg.AAPublic;
using SharpTtsServiceProg.AAPublic;
using SharpTtsServiceProg.Worker;
using SharpVideoServiceProg.AAPublic;

namespace SharpTtsServiceProg.Service
{
    internal class TtsService : ITtsService
    {
        private IOperationsService operationsService;
        private IRepoService repoService;
        private IVideoService videoService;

        private RepoTtsWorker repoTts;
        public RepoTtsWorker RepoTts
        {
            get
            {
                if (!isTtsWorkerInit)
                {
                    TtsWorkerInit();
                    isTtsWorkerInit = true;
                }

                return repoTts;
            }
        }

        public TtsBuilderWorker Tts { get; private set;}

        private bool isTtsWorkerInit;

        private void TtsWorkerInit()
        {
            Tts = new TtsBuilderWorker();
            repoTts = new RepoTtsWorker(operationsService, repoService, videoService);
        }

        public TtsService(
            IOperationsService operationsService,
            IRepoService repoService,
            IVideoService videoService)
        {
            this.operationsService = operationsService;
            this.repoService = repoService;
            this.videoService = videoService;
        }
    }
}