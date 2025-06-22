using System.Text.Json;
using BackendAdapters.Models;
using BackendAdapters.Names;

namespace BackendAdapters.Workers;

public class RepoAdapter
{
    private readonly BackendAdapter _backend;

    public RepoAdapter(
        BackendAdapter backend)
    {
        _backend = backend;
    }
    
    public async Task<(string Repo, string Loca)> GetFirstRepo()
    {
        string jsonStr =
            await _backend.InvokeStringArgsApi(
                SNames.RepoService,
                SNames.Methods,
                SNames.GetFirstRepo);
        string[]? tmp = JsonSerializer.Deserialize<string[]>(jsonStr);
        var result = (tmp[0], tmp[1]);
        return result;
    }
    
    public async Task<string[]> GetAllReposNames()
    {
        string jsonStr =
            await _backend.InvokeStringArgsApi(
                SNames.RepoService,
                SNames.Methods,
                SNames.GetAllReposNames);
        string[]? result = JsonSerializer.Deserialize<string[]>(jsonStr);
        return result ?? new string[0];
    }
    
    public async Task<(bool, ItemModel)> GetItem(
        (string Repo, string Loca) adrTuple)
    {
        string jsonStr = await _backend.InvokeStringArgsApi(
            [SNames.RepoService, SNames.ItemWorker, SNames.GetItem,
                adrTuple.Repo,
                adrTuple.Loca]);
        
        ItemModel item = JsonSerializer.Deserialize<ItemModel>(jsonStr);
        
        if (item == null) return (false, null);
        
        return (true, item);
    }

    public async Task<(bool, ItemModel)> GetByGuid(
        string repoName,
        Guid guid)
    {
        string jsonStr = await _backend.InvokeStringArgsApi(
        [SNames.RepoService, SNames.ItemWorker, SNames.GetByGuid,
            repoName,
            guid.ToString()]);
        
        ItemModel item = JsonSerializer.Deserialize<ItemModel>(jsonStr);
        
        if (item == null) return (false, null);
        
        return (true, item);
    }
}
