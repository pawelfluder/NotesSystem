using SharpRepoServiceProg.AAPublic;
using SharpRepoServiceProg.Workers.AAPublic;

namespace BlazorUniSystemCore.Registrations.Mocks;

public class RepoServiceMock : IRepoService
{
    public JsonWorker Item { get; }
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