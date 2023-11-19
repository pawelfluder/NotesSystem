
using SharpRepoServiceProg.Service;
using SharpTtsServiceProg.Service;

namespace SharpTtsServiceProg.AAPublic
{
    public static class OutBorder
    {
        public static ITtsService TtsService(IRepoService repoService)
        {
            return new TtsService(repoService);
        }
    }
}
