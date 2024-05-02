using SharpRepoServiceProg.Workers;
using System.Collections.Generic;

namespace SharpRepoServiceProg.AAPublic
{
    public partial interface IRepoService
    {
        MethodWorker Methods { get; }

        JsonWorker Item { get; }

        //TextWriteWorker Text { get; }

        //FolderWriteWorker Folder { get; }

        void PutPaths(List<string> searchPaths);
    }
}