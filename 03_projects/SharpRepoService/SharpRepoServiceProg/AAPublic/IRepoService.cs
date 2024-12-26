using System.Collections.Generic;
using SharpRepoServiceProg.Workers.AAPublic;

namespace SharpRepoServiceProg.AAPublic;

public partial interface IRepoService
{
    //ItemWorker Items { get; }

    JsonWorker Item { get; }
    
    MethodWorker Methods { get; }

    //TextWriteWorker Text { get; }

    //FolderWriteWorker Folder { get; }

    void InitGroupsFromSearchPaths(List<string> searchPaths);

    (string Repo, string Loca) GetFirstRepo();
}