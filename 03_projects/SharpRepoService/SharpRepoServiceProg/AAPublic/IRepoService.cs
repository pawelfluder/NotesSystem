using System.Collections.Generic;
using SharpRepoServiceProg.Workers.AAPublic;

namespace SharpRepoServiceProg.AAPublic;

public interface IRepoService
{
    JsonWorker Item { get; }
    
    MethodWorker Methods { get; }

    void InitGroupsFromSearchPaths(List<string> searchPaths);

    (string Repo, string Loca) GetFirstRepo();
}
