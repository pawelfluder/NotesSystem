using SharpRepoServiceProg.RepoOperations;
using System.Collections.Generic;

namespace SharpRepoServiceProg.AAPublic
{
    public partial interface IRepoService
    {
        RepoWorker Methods { get; }

        ItemWorker Item { get; }

        void Initialize(List<string> searchPaths);
    }
}