using System.Collections.Generic;

namespace SharpRepoServiceCoreProj
{
    public class LocalInfo
    {
        public List<string> LocalRootPaths { get; }

        public LocalInfo(List<string> localRootPaths)
        {
            LocalRootPaths = localRootPaths;
        }
    }
}
