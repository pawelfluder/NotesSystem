using System;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Registration;

namespace SharpRepoServiceProg.Workers.CrudReads;

internal class ReadMultiWorker : ReadWorkerBase
{
    private readonly ReadFolderWorker _readFolder;
    private readonly ReadTextWorker _readText;
    private readonly ReadRefWorker _readRef;

    public ReadMultiWorker()
    {
        _readFolder = MyBorder.MyContainer.Resolve<ReadFolderWorker>();
        _readText = MyBorder.MyContainer.Resolve<ReadTextWorker>();
        _readRef = MyBorder.MyContainer.Resolve<ReadRefWorker>();
    }

    public ItemModel GetItem(
        (string Repo, string Loca) adrTuple,
        string type = null)
    {
        if (type == null)
        {
            // todo prevent read of type if not needed!
            type = _config.GetType(adrTuple);
        }
        
        bool isKnownType = Enum.TryParse<UniType>(type, out var uniType);
        ItemModel item = new();
        if (!isKnownType)
        {
            return item;
        }

        item = _readFolder.TryGetItem(item, adrTuple, uniType);
        item = _readText.TryGetItem(item, adrTuple, uniType);
        item = _readRef.TryGetItem(item, adrTuple, uniType);
        return item;
    }
}
