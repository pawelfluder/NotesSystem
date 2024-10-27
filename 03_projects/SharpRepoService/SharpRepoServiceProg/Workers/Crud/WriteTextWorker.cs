using System;
using System.Collections.Generic;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Names;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.Workers.System;

namespace SharpRepoServiceProg.Workers.Crud;

public class WriteTextWorker
{
    private readonly PathWorker pw;
    private readonly SystemWorker _sw;
    private readonly ConfigWorker _cw;
    private readonly BodyWorker _bw;
    private readonly ReadWorker _rw;
    private readonly OperationsService _operationsService;

    public WriteTextWorker()
    {
        _rw = MyBorder.Container.Resolve<ReadWorker>();
        _bw = MyBorder.Container.Resolve<BodyWorker>();
        _cw = MyBorder.Container.Resolve<ConfigWorker>();
        _sw = MyBorder.Container.Resolve<SystemWorker>();
        _operationsService = MyBorder.Container.Resolve<OperationsService>();
    }

    public ItemModel Put(
        string name,
        (string Repo, string Loca) adrTuple,
        string content)
    {
        var item = new ItemModel();

        // config
        var address = _operationsService.UniAddress.CreateUrlFromAddress(adrTuple);
        item.Settings = new Dictionary<string, object>()
        {
            { FieldsForUniItem.Id, Guid.NewGuid().ToString() },
            { FieldsForUniItem.Type, ItemTypeNames.Text },
            { FieldsForUniItem.Name, name },
            { FieldsForUniItem.Address, address }
        };

        // body
        item.Body = content;

        Put(item);
        return item;
    }

    internal ItemModel Put(ItemModel item)
    {
        // directory
        _sw.CreateDirectoryIfNotExists(item.AdrTuple);

        // config
        _cw.CreateConfig(item.AdrTuple, item.Settings);

        // body
        _bw.CreateBody(item.AdrTuple, item.Body.ToString());

        return item;
    }

    public (string Repo, string Loca) Post(
        string name,
        (string Repo, string Loca) adrTuple)
    {
        var item = InternalPost(name, adrTuple);
        return item.AdrTuple;
    }

    internal ItemModel InternalPost(
        string name,
        (string Repo, string Loca) adrTuple)
    {
        ItemModel item = null;
        var foundAdrTuple = _rw.GetAdrTupleByName(adrTuple, name);
        if (foundAdrTuple != default)
        {
            item = _rw.GetItemConfig(foundAdrTuple);
            return item;
        }

        var lastIndex = _rw.GetFolderLastNumber(adrTuple);
        var newIndex = lastIndex + 1;
        var newIndexString = _operationsService.Index.IndexToString(newIndex);

        var newAdrTuple = _operationsService.Index.AdrTupleJoinLoca(adrTuple, newIndexString);
        item = PrepareItem(name, newAdrTuple, "");
        Put(item);
        return item;
    }

    public void Patch(
        string content,
        (string Repo, string Loca) adrTuple)
    {
        var item = _rw.GetItem(adrTuple);
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
        var existingItem = _rw.GetAdrTupleByName(address, name);
        if (existingItem != default)
        {
            Append(existingItem, content);
            return existingItem;
        }

        var lastNumber = _rw.GetFolderLastNumber(address);
        var newAddress = _operationsService.Index.SelectAddress(address, lastNumber + 1);
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
            { FieldsForUniItem.Id, Guid.NewGuid().ToString() },
            { FieldsForUniItem.Type, ItemTypeNames.Text },
            { FieldsForUniItem.Name, name },
        };
        _cw.AddSettingsToModel(item, adrTuple, settings);

        // body
        item.Body = content;

        return item;
    }
}