using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Names;
using SharpRepoServiceProg.Operations;

namespace SharpRepoServiceProg.Workers.CrudReads;

internal class ReadFolderWorker : ReadWorkerBase
{
    private readonly UniType _myType = UniType.Folder;

    // read; body
    public ItemModel GetItemBody(
        (string Repo, string Loca) adrTuple)
    {
        var item = new ItemModel();
        var address = _customOperations.UniAddress
            .CreateAddresFromAdrTuple(adrTuple);
        item.Address = address;
        item.Body = _body.GetBody(adrTuple);
        return item;
    }

    // read; config, body
    public ItemModel TryGetItem(
        ItemModel item,
        (string Repo, string Loca) adrTuple,
        UniType type = UniType.Folder,
        bool addBody = true)
    {
        if (_myType != type) { return item; }
        
        // config
        item.Settings = _migrate
            .GetConfigBeforeRead(adrTuple);

        // body
        item.Body = ListOfIndexesQNames(adrTuple);

        return item;
    }
    
    public object TryGetConfigValue(
        (string Repo, string Loca) adrTuple,
        string key)
    {
        Dictionary<string, object> dict = _config.GetConfigDictionary(adrTuple);
        bool exists = dict.TryGetValue(key, out var value);
        if (exists)
        {
            return value;
        }

        return null;
    }

    public List<string> GetTextLines(
        (string repo, string loca) adrTuple)
    {
        var item = GetItemBody(adrTuple);
        var configLines = item.Body.ToString().Split(_system.NewLine).ToList();
        return configLines;
    }

    // read; config, body
    public (string repo, string newLoca) GetRefAdrTuple(
        (string repo, string loca) adrTuple)
    {
        var keyDict = GetConfigDict(adrTuple, SettingsKeys.RefLocaStr, SettingsKeys.RefGuidStr);
        var guid = keyDict[SettingsKeys.RefGuidStr].ToString();
        var newLoca = keyDict[SettingsKeys.RefLocaStr].ToString();

        var newAdrTuple = (adrTuple.repo, newLoca);
        var id = GetConfigKey(newAdrTuple, "id").ToString();

        if (id == guid)
        {
            return newAdrTuple;
        }

        // todo implement guid search and cache
        (string, string) id2 = default; // FindIdAdrTuple();

        return id2;
    }

    // read; config
    public Dictionary<string, object> GetConfigDict(
        (string Repo, string Loca) address,
        params string[] keyArray)
    {
        var text = _config.GetConfigText(address);
        var success = _yamlOperations
            .TryDeserialize<Dictionary<string, object>>(text, out var configDict);
        var resultDict = new Dictionary<string, object>();

        if (!success)
        {
            return resultDict;
        }

        if (keyArray.Length == 0)
        {
            return configDict;
        }

        foreach (var key in keyArray)
        {
            var success2 = configDict.TryGetValue(key, out var resultValue);

            if (!success2)
            {
                resultDict.Add(key, ErrorValue);
                continue;
            }

            resultDict.Add(key, resultValue);
        }

        return resultDict;
    }

    // read; config
    public List<(int, string)> GetIndexesQNames(
        (string Repo, string Loca) adrTuple)
    {
        var items = _readMany
            .ListOfItemsWithConfig(adrTuple);
        var result = items
            .Select(x => (_customOperations.UniAddress.GetLastLocaIndex(x.Address), x.Name))
            .ToList();
        return result;
    }

    public Dictionary<string, string> ListOfIndexesQNames(
        (string Repo, string Loca) adrTuple)
    {
        List<ItemModel> items = _readMany
            .ListOfItemsWithConfig(adrTuple);
        var kv = items.Select(x => SelectIndexQName(x))
            .ToList();
        
        Dictionary<string, string> dict = kv
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.Value);
        return dict;
    }

    private KeyValuePair<string, string> SelectIndexQName(
        ItemModel x)
    {
        int index = _customOperations.UniAddress
            .GetLastLocaIndex(x.Address);
        string indexString = _customOperations.Index
            .IndexToString(index);
        KeyValuePair<string, string> indexQName = new(indexString, x.Name);
        return indexQName;
    }

    // read; directory
    //public Dictionary<string, string> GetSubAddresses2(
    //    (string Repo, string Loca) adrTuple)
    //{
    //    var itemPath = pw.GetItemPath(adrTuple);
    //    var dirs = sw.GetDirectories(itemPath);
    //    var kv = dirs.Select(x => new KeyValuePair<string, string>(adrTuple.Repo, mw.SelectDirToSection(adrTuple.Loca, x))).ToList();
    //    var dict = kv.ToDictionary(x => x.Key, x => x.Value);
    //    return dict;
    //}

    // read; config, 
    public (string, string) GetFolderByName(
        string repo,
        string loca,
        string name)
    {
        var adrTuple = (repo, loca);
        var items = _readMany
            .ListOfItemsWithConfig(adrTuple)
            .Where(x => x.Type == ItemTypeNames.Folder);
        var found = items.SingleOrDefault(x => x.Name == name);
        if (found == default)
        {
            return default;
        }

        var index = _customOperations.UniAddress
            .GetLastLocaIndex(found.Address);
        var indexString = _customOperations.Index.IndexToString(index);
        var result = (indexString, found.Name);
        return result;
    }

    // read; config
    public List<string> GetConfigLines(
        (string Repo, string Loca) adrTuple)
        => _config.GetConfigLines(adrTuple);

    // read; config
    public bool TryGetConfigLines(
        (string Repo, string Loca) address,
        out List<string> lines)
        => TryGetConfigLines(address, out lines);



    // read; config
    public object TryGetConfigKey(
        (string Repo, string Loca) address,
        string key)
    {
        try
        {
            var gg = GetConfigKey(address, key);
            return gg;
        }
        catch
        {
            return "";
        }
    }

    // read; config
    public object GetConfigKey(
        (string Repo, string Loca) address,
        string key)
    {
        var text = _config.GetConfigText(address);
        var success = _yamlOperations
            .TryDeserialize<Dictionary<string, object>>(text, out var resultDict);

        if (!success)
        {
            return ErrorValue;
        }

        var success2 = resultDict.TryGetValue(key, out var resultValue);

        if (!success2)
        {
            return ErrorValue;
        }

        return resultValue;
    }

    // read; config
    public string GetType(
        (string repo, string loca) adrTuple)
    {
        var type = GetConfigKey(adrTuple, "type").ToString();
        if (type == "RefText")
        {
            return "RefText";
        }

        var contentFilePath = _path.GetBodyPath(adrTuple);
        if (File.Exists(contentFilePath))
        {
            return "Text";
        }

        return "Folder";
    }

    // read; directory
    public int GetFolderLastNumber(
        (string Repo, string Loca) address)
    {
        var directories = _system.GetDirectories(address);
        if (directories.Length == 0)
        {
            return 0;
        }
        
        var numbers = directories
            .Select(x => _customOperations.Index.StringToIndex(Path.GetFileName(x)))
            .ToList();
        if (numbers.Count != 0)
        {
            return numbers.Max();
        }

        return 0;
    }

    // read; directory
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

    public List<string> GetAllReposNames()
    {
        List<string> repos = _path.GetAllReposPaths()
            .Select(x => Path.GetFileName(x))
            .ToList();
        return repos;
    }

    // todo
    public List<string> GetAllRepoAddresses()
    {
        throw new Exception();

        var repos = _path.GetAllReposPaths()
            .Select(x => Path.GetFileName(x)).ToList();
        return repos;
    }

    public string GetText2(
        (string Repo, string Loca) adrTuple)
        => _body.GetBody(adrTuple);
    
    private ItemModel TrySetAddress(
        ItemModel item,
        (string Repo, string Loca) adrTuple)
    {
        if (string.IsNullOrEmpty(item.Address))
        {
            string address = _customOperations.UniAddress
                .CreateAddresFromAdrTuple(adrTuple);
            item.Address = address;
        }
        
        return item;
    }
}