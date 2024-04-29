using SharpRepoServiceProg.RepoOperations;
using System.Collections.Generic;

namespace SharpRepoServiceProg.AAPublic
{
    public partial interface IRepoService
    {
        RepoWorker Methods { get; }

        ItemWorker Item { get; }

        void PutPaths(List<string> searchPaths);
    }
}