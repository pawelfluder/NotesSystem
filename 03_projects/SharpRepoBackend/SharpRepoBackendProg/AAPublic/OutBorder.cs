using SharpRepoBackendProg.Services;

namespace SharpRepoBackendProg.AAPublic;

public class OutBorder
{
    public static IBackendService BackendService()
    {
        return new BackendService();
    }
}