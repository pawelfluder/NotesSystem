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
        private bool repoWorkerInit;
        private bool itemWorkerInit;

        private RepoWorker methods;
        private ItemWorker item;

        private bool isRepoInit;

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
                if (!isRepoInit)
                {
                    methods = new RepoWorker(fileService, serverInfo, localInfo);
                    isRepoInit = true;
                }

                return methods;
            }
        }

        public ItemWorker Item
        {
            get
            {
                if (!itemWorkerInit)
                {
                    var methods = Methods;
                    item = new ItemWorker(methods, fileService);
                    itemWorkerInit = true;
                }

                return item;
            }
        }

        public void PutPaths(List<string> searchPaths)
        {
            Methods.PutPaths(searchPaths);

            if (!(methods.GetReposCount() > 0))
            {
                throw new Exception();
            }
        }
    }
}
