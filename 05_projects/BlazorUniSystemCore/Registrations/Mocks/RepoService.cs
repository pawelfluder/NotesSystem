using SharpRepoServiceProg.AAPublic;
using SharpRepoServiceProg.Workers.AAPublic;
using SharpRepoServiceProg.Workers.APublic;
using ItemWorker = SharpRepoServiceProg.Workers.APublic.ItemWorkers.ItemWorker;

namespace BlazorUniSystemCore.Registrations.Mocks;

public class RepoServiceMock : IRepoService
{
    public ItemWorker Item { get; }
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