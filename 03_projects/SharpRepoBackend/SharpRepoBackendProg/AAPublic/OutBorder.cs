using SharpRepoBackendProg.Service;

namespace SharpRepoBackendProg.AAPublic;

public class OutBorder
{
    public static IBackendService BackendService()
    {
        return new BackendService();
    }
}