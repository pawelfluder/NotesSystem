using SharpFileServiceProg.Service;
using SharpRepoServiceProg.AAPublic;
using SharpTtsServiceProg.Service;
using SharpVideoServiceProg.AAPublic;

namespace SharpTtsServiceProg.AAPublic
{
    public static class OutBorder
    {
        public static ITtsService TtsService(
            IFileService fileService,
            IRepoService repoService,
            IVideoService videoService)
        {
            return new TtsService(fileService, repoService, videoService);
        }
    }
}
