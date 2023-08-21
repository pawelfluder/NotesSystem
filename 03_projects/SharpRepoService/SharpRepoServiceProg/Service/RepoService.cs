using System;
using System.Collections.Generic;
using SharpFileServiceProg.Service;
using SharpRepoServiceCoreProj;
using SharpRepoServiceProg.RepoOperations;

namespace SharpRepoServiceProg.Service
{
    internal class RepoService : IRepoService
    {
        private readonly ServerInfo serverInfo;
        private readonly LocalInfo localInfo;
        private bool initialized;
        private RepoServiceMethods methods;

        internal RepoService(
            IFileService fileService)
        {
            localInfo = new LocalInfo(null);
            serverInfo = null;
            Methods = new RepoServiceMethods(fileService, serverInfo, localInfo);
        }

        public RepoServiceMethods Methods
        {
            get
            {
                if (!initialized) { throw new Exception(); }
                return methods;
            }
            private set => methods = value;
        }

        public void Initialize(List<string> searchPaths)
        {
            if (initialized) { throw new Exception(); }
            methods.Initialize(searchPaths);
            initialized = true;
            if (!(methods.GetReposCount() > 0))
            {
                throw new Exception();
            }
        }
    }
}
