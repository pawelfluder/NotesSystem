using System;
using System.Collections.Generic;
using SharpFileServiceProg.Service;
using SharpRepoServiceCoreProj;
using SharpRepoServiceProg.AAPublic;
using SharpRepoServiceProg.RepoOperations;

namespace SharpRepoServiceProg.Service
{
    internal class RepoService : IRepoService
    {
        private readonly ServerInfo serverInfo;
        private readonly IFileService fileService;
        private readonly LocalInfo localInfo;
        private bool repoWorkerIsInit;
        private RepoWorker methods;
        private ItemWorker item;

        internal RepoService(
            IFileService fileService)
        {
            localInfo = new LocalInfo(null);
            serverInfo = null;
            this.fileService = fileService;
        }

        public RepoWorker Methods
        {
            get
            {
                InitializeWorkers();
                return methods;
            }
        }

        public ItemWorker Item
        {
            get
            {
                InitializeWorkers();
                return item;
            }
        }

        private void InitializeWorkers()
        {
            if (methods == null)
            {
                methods = new RepoWorker(fileService, serverInfo, localInfo);
            }

            if (item == null)
            {
                item = new ItemWorker(methods, fileService);
            }
        }

        public void Initialize(List<string> searchPaths)
        {
            InitializeWorkers();

            if (repoWorkerIsInit) { throw new Exception(); }
            methods.Initialize(searchPaths);
            repoWorkerIsInit = true;
            if (!(methods.GetReposCount() > 0))
            {
                throw new Exception();
            }
        }
    }
}
