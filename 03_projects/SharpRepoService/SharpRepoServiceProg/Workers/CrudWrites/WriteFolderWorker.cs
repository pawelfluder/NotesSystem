using System;
using System.Collections.Generic;
using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.Workers.CrudReads;
using SharpRepoServiceProg.Workers.System;

namespace SharpRepoServiceProg.Workers.CrudWrites;

public class WriteFolderWorker : WriteWorkerBase
{
    private UniType _myUniType = UniType.Folder;

    public ItemModel Put(
        string name,
        (string Repo, string Loca) adrTuple)
    {
        ItemModel item = new();

        // config
        item.Settings = new Dictionary<string, object>()
        {
            { ConfigKeys.Id, Guid.NewGuid().ToString() },
            { ConfigKeys.Type, ItemTypeNames.Folder },
            { ConfigKeys.Name, name }
        };
        
        Put(item);
        return item;
    }
    
    internal ItemModel Put(ItemModel item)
    {
        // directory
        _system.CreateDirectoryIfNotExists(item.AdrTuple);

        // config
        _config.PutConfig(item.AdrTuple, item.Settings);

        return item;
    }

    public (string Repo, string Loca) Post(
        string name,
        (string Repo, string Loca) adrTuple)
    {
        var foundAdrTuple = _address.GetAdrTupleByName(adrTuple, name);
        if (foundAdrTuple != default)
        {
            Put(name, foundAdrTuple);
            return foundAdrTuple;
        }

        var lastIndex = _readFolder.GetFolderLastNumber(adrTuple);
        var newIndex = lastIndex + 1;
        var newIndexString = _customOperations.Index.IndexToString(newIndex);

        var newAdrTuple = _customOperations.Index.AdrTupleJoinLoca(adrTuple, newIndexString);
        Put(name, newAdrTuple);
        return newAdrTuple;
    }

    internal ItemModel TryInternalPost(
        ItemModel item,
        string name,
        (string Repo, string Loca) adrTuple,
        UniType uniType)
    {
        if (uniType != _myUniType) { return item; }
        
        var foundAdrTuple = _address.GetAdrTupleByName(adrTuple, name);
        if (foundAdrTuple != default)
        {
            item = _migrate.GetItemWithConfig(adrTuple);
            Put(name, foundAdrTuple);
            return item;
        }

        var lastIndex = _readFolder.GetFolderLastNumber(adrTuple);
        var newIndex = lastIndex + 1;
        var newIndexString = _customOperations.Index.IndexToString(newIndex);

        var newAdrTuple = _customOperations.Index.AdrTupleJoinLoca(adrTuple, newIndexString);
        item = PrepareItem(name, newAdrTuple);
        Put(item);
        return item;
    }

    private ItemModel PrepareItem(
        string name,
        (string Repo, string Loca) adrTuple)
    {
        var item = new ItemModel();

        // config
        var settings = new Dictionary<string, object>()
        {
            { ConfigKeys.Id, Guid.NewGuid().ToString() },
            { ConfigKeys.Type, ItemTypeNames.Folder },
            { ConfigKeys.Name, name },
        };
        item.Settings = _migrate.GetConfigBeforeWrite(settings, adrTuple);
        return item;
    }
}
