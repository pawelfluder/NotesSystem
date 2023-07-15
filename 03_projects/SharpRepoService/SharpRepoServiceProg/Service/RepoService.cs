using System.Collections.Generic;
using SharpFileServiceProg.Service;
using SharpRepoServiceCoreProj;
using SharpRepoServiceProg.RepoOperations;

namespace SharpRepoServiceProg.Service
{
    public class RepoService : IRepoService
    {
        private readonly ServerInfo serverInfo;
        private readonly LocalInfo localInfo;

        public RepoServiceMethods Methods { get; private set; }

        public RepoService(
            IFileService fileService,
            List<string> localRootPaths)
        {
            localInfo = new LocalInfo(localRootPaths);
            serverInfo = null;
            Methods = new RepoServiceMethods(fileService, serverInfo, localInfo);
        }

        public RepoService(IFileService fileService)
        {
            serverInfo = new ServerInfo();
            localInfo = new LocalInfo(new List<string>());
            Methods = new RepoServiceMethods(fileService, serverInfo, localInfo);
        }
    }
}
