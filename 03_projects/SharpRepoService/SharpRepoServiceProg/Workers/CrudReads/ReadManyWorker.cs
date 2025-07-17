using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Duplications.Operations;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Registrations;
using SharpRepoServiceProg.Workers.CrudWrites;
using SharpRepoServiceProg.Workers.System;

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
    private PathWorker _path;
    private ReadMultiWorker _readMulti;

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
        List<(string, string)> adrTupleList = _address
            .GetSubAdrTuples(adrTuple);
        List<ItemModel> items = new();
        foreach (var adr in adrTupleList)
        {
            try
            {
                ItemModel item = new();
                bool s01 = _readMulti.GetItem(ref item, adr);
                if (s01)
                {
                    items.Add(item);
                }
            }
            catch(Exception ex)
            {}
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
            _path = MyBorder.MyContainer.Resolve<PathWorker>();
            _readMulti = MyBorder.MyContainer.Resolve<ReadMultiWorker>();
            isInitialized = true;
        }
    }
    
    public List<string> GetAllRepoAddresses()
    {
        var repos = _path.GetAllReposPaths()
            .Select(x => Path.GetFileName(x)).ToList();
        return repos;
    }
    
    public List<(string Repo, string Loca)> GetAllRepoAddresses(
        string repoName)
    {
        var adrTuple = (repoName, "");
        var path = _path.GetItemPath(adrTuple);
        var tmp = _customOperations.File.NewRepoAddressesObtainer().Visit(path);
        var result = tmp
            .Select(x => (adrTuple.Item1, _customOperations.UniAddress.JoinLoca(adrTuple.Item2, x)))
            .ToList();
        return result;
    }

    // read; config
    public List<ItemModel> ListOfOnlyConfigItems(
        (string Repo, string Loca) adrTuple)
    {
        TryInitialize();
        var adrTupleList = _address.GetSubAdrTuples(adrTuple);
        var items = new List<ItemModel>();
        foreach (var adr in adrTupleList)
        {
            if (_helper.IsSpecialFolder(adr))
            {
                continue;
            }
            ItemModel item = _migrate.GetOnlyItemConfig(adr);
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
    public List<string> GetManyItemsBody(
        (string Repo, string Loca) adrTuple)
    {
        TryInitialize();
        var items = GetListOfItems(adrTuple);
        var contentsList = new List<string>();

        foreach (var item in items)
        {
            if (item.Type == UniType.Text.ToString())
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
            if (item.Type == UniType.Text.ToString())
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
        List<string> contentsList = GetManyItemsBody(newAdrTuple);

        return contentsList;
    }
    
    public (string, string) GetAdrTupleByName(
        (string Repo, string Loca) adrTuple,
        string name)
    {
        var items = ListOfOnlyConfigItems(adrTuple);
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
