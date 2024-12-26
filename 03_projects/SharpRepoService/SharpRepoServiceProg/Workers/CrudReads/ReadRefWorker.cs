using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Models;

namespace SharpRepoServiceProg.Workers.CrudReads;
internal class ReadRefWorker : ReadWorkerBase
{
    private readonly UniType _myType = UniType.Ref;
    public ItemModel TryGetItem(
        ItemModel item,
        (string Repo, string Loca) adrTuple,
        UniType uniType)
    {
        if (_myType != uniType) { return item; }
        
        // config
        // item.Settings = _migrate
        //     .GetConfigBeforeRead(adrTuple);
        //
        // // body
        // item.Body = _body.GetBody(adrTuple);

        return item;
    }
}
