using SharpOperationsProg.AAPublic.Operations;
using SharpRepoServiceProg.AAPublic;
using SharpTtsServiceProg.Services;
using SharpVideoServiceProg.AAPublic;

namespace SharpTtsServiceProg.AAPublic;

public static class OutBorder
{
    private static bool _wasTaken;
    public static ITtsService TtsService(
        IOperationsService operationsService,
        IRepoService repoService,
        IVideoService videoService)
    {
        if (_wasTaken)
        {
            throw new InvalidOperationException("Service can only be taken once");
        }
        
        _wasTaken = true;
        return new TtsService(operationsService, repoService, videoService);
    }
}
