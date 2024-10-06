using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Names;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.Workers.System;

namespace SharpRepoServiceProg.Workers.Crud;

internal class ReadWorker
{
    private IFileService _fileService;
    private readonly PathWorker _pw;
    private readonly BodyWorker _bw;
    private readonly ConfigWorker _cw;
    private readonly SystemWorker _sw;
    private readonly MemoryWorker _mw;

    private readonly IYamlOperations _yamlOperations;
    private readonly OperationsService _operationsService;

    public object ErrorValue { get; internal set; }

    public ReadWorker()
    {
        _fileService = MyBorder.Container.Resolve<IFileService>();
        _operationsService = MyBorder.Container.Resolve<OperationsService>();
        _yamlOperations = _fileService.Yaml.Custom03;
        _pw = MyBorder.Container.Resolve<PathWorker>();
        _bw = MyBorder.Container.Resolve<BodyWorker>();
        _cw = MyBorder.Container.Resolve<ConfigWorker>();
        _sw = MyBorder.Container.Resolve<SystemWorker>();
        _mw = MyBorder.Container.Resolve<MemoryWorker>();
    }

    // read; body
    public ItemModel GetItemBody(
        (string Repo, string Loca) adrTuple)
    {
        var item = new ItemModel();
        var address = _operationsService.UniAddress
            .CreateUrlFromAddress(adrTuple);
        //item.AdrTuple = adrTuple;
        item.Body = _bw.GetText2(adrTuple);
        return item;
    }

    // read; config, body
    public ItemModel GetItem(
        (string Repo, string Loca) adrTuple,
        bool IncludeSubFolder = false)
    {
        var item = new ItemModel();
        //item.AdrTuple = adrTuple;

        // config
        var settings =  GetConfigDict(adrTuple);
        _cw.AddSettingsToModel(item, adrTuple, settings);

        // body
        if (item.Type == ItemTypes.Text)
        {
            var body = _bw.GetText2(adrTuple);
            item.Body = body;
        }

        if (item.Type == ItemTypes.Folder &&
            IncludeSubFolder)
        {
            item.Body = GetIndexesQNames2(adrTuple);
        }

        return item;
    }

    // read; config, body
    public List<ItemModel> GetItemList(
        (string Repo, string Loca) adrTuple)
    {
        var adrTupleList = GetSubAddresses(adrTuple);
        var items = new List<ItemModel>();
        foreach (var adr in adrTupleList)
        {
            var item = GetItem(adr);
            items.Add(item);
        }

        return items;
    }

    public object TryGetConfigValue(
        (string Repo, string Loca) adrTuple,
        string key)
    {
        var dict = _cw.GetConfigDictionary(adrTuple);
        var exists = dict.TryGetValue(key, out var value);
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
        var configLines = item.Body.ToString().Split(_sw.NewLine).ToList();
        return configLines;
    }

    // read; config, body
    public List<(int, string)> GetManyIdxQTextByNames(
        (string Repo, string Loca) adrTuple,
        params string[] names)
    {
        var newAdrTuple = GetAdrTupleByNames(adrTuple, names);
        var idxQTextList = GetManyIdxQText(newAdrTuple);

        return idxQTextList;
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

    // read; config, body
    public List<string> GetManyText(
        (string Repo, string Loca) adrTuple)
    {
        var items = GetItemList(adrTuple);
        var contentsList = new List<string>();

        foreach (var item in items)
        {
            if (item.Type == ItemTypes.Text)
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
        var items = GetItemList(adrTuple);
        var contentsList = new List<(int, string)>();

        foreach (var item in items)
        {
            if (item.Type == ItemTypes.Text)
            {
                var index = _operationsService.UniAddress
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
        var newAdrTuple = GetAdrTupleByNames(adrTuple, names);
        var contentsList = GetManyText(newAdrTuple);

        return contentsList;
    }

    // read; config
    public ItemModel GetItemConfig(
        (string Repo, string Loca) adrTuple)
    {
        var item = new ItemModel();
        var settings = GetConfigDict(adrTuple);
        _cw.AddSettingsToModel(item, adrTuple, settings);
        return item;
    }

    // read; config
    public Dictionary<string, object> GetConfigDict(
        (string Repo, string Loca) address,
        params string[] keyArray)
    {
        var text = _cw.GetConfigText(address);
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
        var items = GetItemConfigList(adrTuple);
        var result = items
            .Select(x => (_operationsService.UniAddress.GetLastLocaIndex(x.Address), x.Name))
            .ToList();
        return result;
    }

    public Dictionary<string, string> GetIndexesQNames2(
        (string Repo, string Loca) adrTuple)
    {
        var w1 = _operationsService.UniAddress;
        var w2 = _operationsService.Index;

        var items = GetItemConfigList(adrTuple);
        var kv = items
            .Select(x => new KeyValuePair<string, string>(w2.IndexToString(w1.GetLastLocaIndex(x.Address)), x.Name));
            
        var dict = kv
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.Value);
        return dict;
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

    // read; config
    public List<ItemModel> GetItemConfigList(
        (string Repo, string Loca) adrTuple)
    {
        var adrTupleList = GetSubAddresses(adrTuple);
        var items = new List<ItemModel>();
        foreach (var adr in adrTupleList)
        {
            if (IsSpecialFolder(adr))
            {
                continue;
            }
            var item = GetItemConfig(adr);
            items.Add(item);
        }

        return items;
    }

    private bool IsSpecialFolder((string, string) adr)
    {
        var special = new List<string> { ".git" };
        if (special.Any(x => x == adr.Item1) ||
            special.Any(x => x == adr.Item2))
        {
            return true;
        }
        
        return false;
    }

    // read; config, 
    public (string, string) GetFolderByName(
        string repo,
        string loca,
        string name)
    {
        var adrTuple = (repo, loca);
        var items = GetItemConfigList(adrTuple)
            .Where(x => x.Type == ItemTypeNames.Folder);
        var found = items.SingleOrDefault(x => x.Name == name);
        if (found == default)
        {
            return default;
        }

        var index = _operationsService.UniAddress
            .GetLastLocaIndex(found.Address);
        var indexString = _operationsService.Index.IndexToString(index);
        var result = (indexString, found.Name);
        return result;
    }

    // read; config
    public List<string> GetConfigLines(
        (string Repo, string Loca) adrTuple)
        => _cw.GetConfigLines(adrTuple);

    // read; config
    public bool TryGetConfigLines(
        (string Repo, string Loca) address,
        out List<string> lines)
        => TryGetConfigLines(address, out lines);

    // read; config
    public (string, string) GetAdrTupleByName(
        (string Repo, string Loca) adrTuple,
        string name)
    {
        var items = GetItemConfigList(adrTuple);
        var found = items.SingleOrDefault(x => x.Name.ToString() == name);
        if (found == null)
        {
            return default;
        }

        var foundAdrTuple = _operationsService.UniAddress
            .CreateAddressFromString(found.Address);
        return foundAdrTuple;
    }

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
        var text = _cw.GetConfigText(address);
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

        var contentFilePath = _pw.GetBodyPath(adrTuple);
        if (File.Exists(contentFilePath))
        {
            return "Text";
        }

        return "Folder";
    }

    // read; config
    public (string, string) GetAdrTupleByNames(
        (string Repo, string Loca) adrTuple,
        params string[] names)
    {
        foreach (var name in names)
        {
            adrTuple = GetAdrTupleByName(adrTuple, name);

            if (adrTuple == default)
            {
                return default;
            }
        }

        return adrTuple;
    }

    // read; directory
    public int GetFolderLastNumber(
        (string Repo, string Loca) address)
    {
        var directories = _sw.GetDirectories(address);
        var numbers = directories
            .Select(x => _operationsService.Index.StringToIndex(Path.GetFileName(x)))
            .ToList();
        if (numbers.Count != 0)
        {
            return numbers.Max();
        }

        return 0;
    }

    // read; directory
    public List<(string, string)> GetSubAddresses(
        (string Repo, string Loca) adrTuple)
    {
        var itemPath = _pw.GetItemPath(adrTuple);
        var dirs = _sw.GetDirectories(itemPath);
        var subAddresses = dirs.Select(x => (adrTuple.Repo, _mw.SelectDirToSection(adrTuple.Loca, x))).ToList();
        return subAddresses;
    }

    // read; directory
    public List<(string Repo, string Loca)> GetAllRepoAddresses(
        string repoName)
    {
        var adrTuple = (repoName, "");
        var path = _pw.GetItemPath(adrTuple);
        var tmp = _operationsService.File.NewRepoAddressesObtainer().Visit(path);
        var result = tmp
            .Select(x => (adrTuple.Item1, _operationsService.UniAddress.JoinLoca(adrTuple.Item2, x)))
            .ToList();
        return result;
    }

    public List<string> GetAllReposNames()
    {
        var repos = _pw.GetAllReposPaths()
            .Select(x => Path.GetFileName(x)).ToList();
        return repos;
    }

    // todo
    public List<string> GetAllRepoAddresses()
    {
        throw new Exception();

        var repos = _pw.GetAllReposPaths()
            .Select(x => Path.GetFileName(x)).ToList();
        return repos;
    }

    public string GetText2(
        (string Repo, string Loca) adrTuple)
        => _bw.GetText2(adrTuple);
}