using SharpFileServiceProg.Service;
using SharpRepoServiceProg.AAPublic;
using SharpTtsServiceProg.AAPublic;
using SharpTtsServiceProg.Worker;
using SharpVideoServiceProg.AAPublic;

namespace SharpTtsServiceProg.Service
{
    internal class TtsService : ITtsService
    {
        private IFileService fileService;
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
            repoTts = new RepoTtsWorker(fileService, repoService, videoService);
        }

        public TtsService(
            IFileService fileService,
            IRepoService repoService,
            IVideoService videoService)
        {
            this.fileService = fileService;
            this.repoService = repoService;
            this.videoService = videoService;
        }
    }
}