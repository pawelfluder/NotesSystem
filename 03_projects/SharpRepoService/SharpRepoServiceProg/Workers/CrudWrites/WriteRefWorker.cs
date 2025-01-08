using System;
using System.Collections.Generic;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Workers.CrudReads;

namespace SharpRepoServiceProg.Workers.CrudWrites;

public class WriteRefWorker : WriteWorkerBase
{
    private UniType _myType = UniType.Text;

    public ItemModel Put(
        string name,
        (string Repo, string Loca) adrTuple,
        string content)
    {
        ItemModel item = new();

        // config
        string address = _customOperations.UniAddress
            .CreateAddresFromAdrTuple(adrTuple);
        item.Settings = new Dictionary<string, object>()
        {
            { ConfigKeys.Id, Guid.NewGuid().ToString() },
            { ConfigKeys.Type, ItemTypeNames.Text },
            { ConfigKeys.Name, name },
            { ConfigKeys.Address, address }
        };

        // body
        item.Body = content;

        Put(item);
        return item;
    }

    internal ItemModel Put(ItemModel item)
    {
        // directory
        _system.CreateDirectoryIfNotExists(item.AdrTuple);

        // config
        _config.PutConfig(item.AdrTuple, item.Settings);

        // body
        _body.CreateBody(item.AdrTuple, item.Body.ToString());

        return item;
    }

    public (string Repo, string Loca) Post(
        string name,
        (string Repo, string Loca) adrTuple)
    {
        ItemModel item = TryInternalPost(null, name, adrTuple, _myType);
        return item.AdrTuple;
    }

    internal ItemModel TryInternalPost(
        ItemModel item,
        string name,
        (string Repo, string Loca) adrTuple,
        UniType type)
    {
        if (type != _myType) { return item; }
        
        (string, string) foundAdrTuple = _address
            .GetAdrTupleByName(adrTuple, name);
        if (foundAdrTuple != default)
        {
            item = _migrate.GetItemWithConfig(foundAdrTuple);
            return item;
        }

        int lastIndex = _readFolder.GetFolderLastNumber(adrTuple);
        int newIndex = lastIndex + 1;
        string newIndexString = _customOperations.Index
            .IndexToString(newIndex);

        (string, string) newAdrTuple = _customOperations.Index
            .AdrTupleJoinLoca(adrTuple, newIndexString);
        item = PrepareItem(name, newAdrTuple, "");
        Put(item);
        return item;
    }

    public void Patch(
        string content,
        (string Repo, string Loca) adrTuple)
    {
        ItemModel item = null;
        item = _readFolder.TryGetItem(item, adrTuple);
        item = _readText.TryGetItem(item, adrTuple);
        if (item == default)
        {
            throw new Exception();
        }

        item.Body = content;
        Put(item);
    }

    public (string, string) Append(
        (string Repo, string Loca) address,
        string name,
        string content)
    {
        var existingItem = _address.GetAdrTupleByName(address, name);
        if (existingItem != default)
        {
            Append(existingItem, content);
            return existingItem;
        }

        var lastNumber = _readFolder.GetFolderLastNumber(address);
        var newAddress = _customOperations.Index.SelectAddress(address, lastNumber + 1);
        Put(name, newAddress, content);
        return newAddress;
    }

    public void Append(
        (string Repo, string Loca) address,
        string content)
    {
        // todo
        //AppendTextGenerate(address, content);
    }

    private ItemModel PrepareItem(
        string name,
        (string Repo, string Loca) adrTuple,
        string content)
    {
        var item = new ItemModel();

        // config
        var settings = new Dictionary<string, object>()
        {
            { ConfigKeys.Id, Guid.NewGuid().ToString() },
            { ConfigKeys.Type, ItemTypeNames.Text },
            { ConfigKeys.Name, name },
        };
        item.Settings = _migrate.GetConfigBeforeWrite(settings, adrTuple);

        // body
        item.Body = content;

        return item;
    }
}
