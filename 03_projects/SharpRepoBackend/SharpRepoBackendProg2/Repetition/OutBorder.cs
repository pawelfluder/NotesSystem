using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;

namespace SharpRepoBackendProg.Repetition
{
    public class OutBorder
    {
        public static IBackendService BackendService()
        {
            return new BackendService();
        }
    }
}
