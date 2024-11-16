using SharpOperationsProg.AAPublic.Operations;
using SharpRepoServiceProg.AAPublic;
using SharpTtsServiceProg.Service;
using SharpVideoServiceProg.AAPublic;

namespace SharpTtsServiceProg.AAPublic;

public static class OutBorder
{
    public static ITtsService TtsService(
        IOperationsService operationsService,
        IRepoService repoService,
        IVideoService videoService)
    {
        return new TtsService(operationsService, repoService, videoService);
    }
}