using Newtonsoft.Json;
using SharpRepoServiceProg.Models;

namespace SharpRepoServiceProg.Workers.APublic.ItemWorkers;

public partial class ItemWorker
{
    public string PostParentItem(
        string repo,
        string loca,
        string type,
        string name)
    {
        ItemModel item = new();
        (string Repo, string Loca) adrTuple = new(repo, loca);
        bool s01 = _writeMulti.PostItem(ref item, adrTuple, type, name);
        string result = JsonConvert.SerializeObject(item, Formatting.Indented);
        return result;
    }
}
