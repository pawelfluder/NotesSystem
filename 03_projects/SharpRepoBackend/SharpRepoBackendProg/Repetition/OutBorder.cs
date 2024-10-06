using SharpRepoBackendProg2.Service;

namespace SharpRepoBackendProg2.Repetition;

public class OutBorder
{
    public static IBackendService BackendService()
    {
        return MyBorder.Container.Resolve<IBackendService>();
    }
}