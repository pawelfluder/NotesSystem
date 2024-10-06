using System.Collections.Generic;
using SharpRepoServiceProg.Workers.Public;

namespace SharpRepoServiceProg.AAPublic;

public partial interface IRepoService
{
    //ItemWorker Items { get; }

    JsonWorker Item { get; }
    
    MethodWorker Methods { get; }

    //TextWriteWorker Text { get; }

    //FolderWriteWorker Folder { get; }

    void PutPaths(List<string> searchPaths);
}