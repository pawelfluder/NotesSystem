using SharpRepoBackendProg.Service;
using Unity;

namespace SharpRepoBackendProg.Repetition
{
    public class OutBorder
    {
        public static IBackendService BackendService()
        {
            return MyBorder.Container.Resolve<IBackendService>();
        }
    }
}
