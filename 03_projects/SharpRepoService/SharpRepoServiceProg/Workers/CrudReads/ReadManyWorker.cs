using System;
using System.Collections.Generic;
using System.Linq;
using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.Workers.CrudWrites;

namespace SharpRepoServiceProg.Workers.CrudReads;

internal class ReadManyWorker
{
    private IFileService _fileService;
    private ReadFolderWorker _readFolder;
    private ReadTextWorker _readText;
    
    private readonly CustomOperationsService _customOperations;
    private ReadAddressWorker _address;
    private MigrationWorker _migrate;
    private bool isInitialized;
    private ReadHelper _helper;

    public ReadManyWorker()
    {
        _fileService = MyBorder.OutContainer.Resolve<IFileService>();
        _customOperations = MyBorder.MyContainer.Resolve<CustomOperationsService>();
        _helper = MyBorder.MyContainer.Resolve<ReadHelper>();
    }
    
    // read; config, body
    public List<ItemModel> GetListOfItems(
        (string Repo, string Loca) adrTuple)
    {
        TryInitialize();
        List<(string, string)> adrTupleList = _address.GetSubAddresses(adrTuple);
        List<ItemModel> items = new();
        foreach (var adr in adrTupleList)
        {
            ItemModel item = null;
            item = _readFolder.TryGetItem(item, adrTuple);
            item = _readText.TryGetItem(item, adrTuple);
            items.Add(item);
        }

        return items;
    }

    private void TryInitialize()
    {
        if (!isInitialized)
        {
            _readFolder = MyBorder.MyContainer.Resolve<ReadFolderWorker>();
            _readText = MyBorder.MyContainer.Resolve<ReadTextWorker>();
            _address = MyBorder.MyContainer.Resolve<ReadAddressWorker>();
            _migrate = MyBorder.MyContainer.Resolve<MigrationWorker>();
            isInitialized = true;
        }
    }

    // read; config
    public List<ItemModel> ListOfItemsWithConfig(
        (string Repo, string Loca) adrTuple)
    {
        TryInitialize();
        var adrTupleList = _address.GetSubAddresses(adrTuple);
        var items = new List<ItemModel>();
        foreach (var adr in adrTupleList)
        {
            if (_helper.IsSpecialFolder(adr))
            {
                continue;
            }
            ItemModel item = _migrate.GetItemWithConfig(adr);
            items.Add(item);
        }

        return items;
    }
    
    // read; config, body
    public List<(int, string)> GetManyIdxQTextBySequenceOfNames(
        (string Repo, string Loca) adrTuple,
        params string[] sequenceOfNames)
    {
        TryInitialize();
        (string, string) newAdrTuple = _address.
            GetAdrTupleBySequenceOfNames(adrTuple, sequenceOfNames);
        List<(int, string)> idxQTextList = GetManyIdxQText(newAdrTuple);

        return idxQTextList;
    }

    // read; config, body
    public List<string> GetManyText(
        (string Repo, string Loca) adrTuple)
    {
        TryInitialize();
        var items = GetListOfItems(adrTuple);
        var contentsList = new List<string>();

        foreach (var item in items)
        {
            if (item.Type == UniItemTypes.Text)
            {
                contentsList.Add(item.Body.ToString());
            }
        }

        return contentsList;
    }

    // read; config, body
    public List<(int, string)> GetManyIdxQText(
        (string Repo, string Loca) adrTuple)
    {
        TryInitialize();
        var items = GetListOfItems(adrTuple);
        List<(int, string)> contentsList = new();

        foreach (var item in items)
        {
            if (item.Type == UniItemTypes.Text)
            {
                int index = _customOperations.UniAddress
                    .GetLastLocaIndex(item.Address);
                contentsList.Add((index, item.Body.ToString()));
            }
        }

        return contentsList;
    }

    // read; config
    public List<string> GetManyTextByNames(
        (string Repo, string Loca) adrTuple,
        params string[] names)
    {
        (string, string) newAdrTuple = _address
            .GetAdrTupleBySequenceOfNames(adrTuple, names);
        List<string> contentsList = GetManyText(newAdrTuple);

        return contentsList;
    }
    
    public (string, string) GetAdrTupleByName(
        (string Repo, string Loca) adrTuple,
        string name)
    {
        var items = ListOfItemsWithConfig(adrTuple);
        var found = items.SingleOrDefault(x => x.Name.ToString() == name);
        if (found == null)
        {
            return default;
        }

        var foundAdrTuple = _customOperations.UniAddress
            .CreateAddressFromString(found.Address);
        return foundAdrTuple;
    }
    
    // read; config, body
    public List<(int, string)> GetManyIdxQTextByNames(
        (string Repo, string Loca) adrTuple,
        params string[] names)
    {
        TryInitialize();
        (string, string) newAdrTuple = _address.GetAdrTupleBySequenceOfNames(adrTuple, names);
        List<(int, string)> idxQTextList = GetManyIdxQText(newAdrTuple);

        return idxQTextList;
    }
}
