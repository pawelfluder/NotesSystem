using System.Collections.Generic;
using SharpFileServiceProg.Service;
using SharpRepoServiceCoreProj;

namespace SharpRepoServiceProg.Service
{
    public class RepoService
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

        public RepoService(FileService fileService)
        {
            serverInfo = new ServerInfo();
            localInfo = new LocalInfo(new List<string>());
            Methods = new RepoServiceMethods(fileService, serverInfo, localInfo);
        }
    }
}
