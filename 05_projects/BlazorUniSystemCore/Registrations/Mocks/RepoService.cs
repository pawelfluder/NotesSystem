using SharpRepoServiceProg.AAPublic;
using SharpRepoServiceProg.Workers.APublic;
using SharpRepoServiceProg.Workers.APublic.ItemWorkers;
using ItemWorker = SharpRepoServiceProg.Workers.APublic.ItemWorkers.ItemWorker;

namespace BlazorUniSystemCore.Registrations.Mocks;

public class RepoServiceMock : IRepoService
{
    public IItemWorker Item { get; }
    public ManyItemsWorker ManyItems { get; }
    public MethodWorker Methods { get; }
    public void InitGroupsFromSearchPaths(List<string> searchPaths)
    {
        throw new NotImplementedException();
    }

    public (string Repo, string Loca) GetFirstRepo()
    {
        throw new NotImplementedException();
    }
}

public class MethodWorkerMock
{
}