using SharpRepoServiceProg.AAPublic;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.Workers.CrudWrites;
using NotImplementedException = System.NotImplementedException;

namespace SharpRepoServiceProg.Workers.CrudReads;

public class GuidWorker // : ReadWorkerBase
{
    private ReadTextWorker _readText;
    private WriteFolderWorker _writeFolder;
    
    public string GetAdrTupleByGuid(
        string repoName,
        string guid)
    {
        (string Repo, string) hiddenAdrTuple = (repoName, "00");
        (string Repo, string) cacheAdrTuple = (repoName, "00/01");
        _writeFolder.Post("hidden", hiddenAdrTuple);
        _writeFolder.Post("cache", cacheAdrTuple);
        
        ItemModel item = new();
        ItemModel cache = _readText.TryGetItem(item, cacheAdrTuple);

        return string.Empty;
    }

    private void CreateIfCacheNotExists(
        (string Repo, string) cacheAdrTuple)
    {
        
    }

    private void TryInitialize()
    {
        _readText = MyBorder.MyContainer.Resolve<ReadTextWorker>();
        _writeFolder = MyBorder.MyContainer.Resolve<WriteFolderWorker>();
    }
    
}