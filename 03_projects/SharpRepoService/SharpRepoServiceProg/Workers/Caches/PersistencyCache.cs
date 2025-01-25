using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.AAPublic;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;

namespace SharpRepoServiceProg.Workers.Caches;

public class PersistencyCache : IPersistencyCache
{
    private CustomOperationsService _operationsService;
    private IFileService _fileService;
    private IRepoService _repoService;
    private (string, string) _parentAdrTuple;
    private (string, string) _cacheAdrTuple;

    private Dictionary<string, Dictionary<string, object>> _persistencyDict = new();

    public string KeyString => "key";
    public (string Repo, string Loca) ParentAdrTuple => _parentAdrTuple;
    public (string Repo, string Loca) CacheAdrTuple => _cacheAdrTuple;

    public PersistencyCache(
        (string, string) parentAdrTuple,
        bool performLoad = true)
    {
        _operationsService = MyBorder.OutContainer.Resolve<CustomOperationsService>();
        _fileService = MyBorder.OutContainer.Resolve<IFileService>();
        _repoService = MyBorder.OutContainer.Resolve<IRepoService>();
        
        _parentAdrTuple = parentAdrTuple;
        _cacheAdrTuple = _repoService.Methods
            .PostText(parentAdrTuple, "cache");
        if (performLoad)
        {
            Load();
        }
    }

    public int Count() => _persistencyDict.Count;

    public Dictionary<string, object> Get(string key)
    {
        bool success = _persistencyDict.TryGetValue(key, out var dict);

        if (!success)
        {
            return default;
        }

        return new Dictionary<string, object>(dict);
    }

    public List<Dictionary<string, object>> GetAll()
    {
        var persistencyList = _persistencyDict.Values.ToList();
        return persistencyList;
    }

    public bool PutQSave(
        string mainKeyString,
        Dictionary<string, object> dict)
    {
        bool success = Put(mainKeyString, dict);
        if (success)
        {
            Save();
            return true;
        }
        
        return false;
    }
    
    public bool Put(
        string mainKeyString,
        Dictionary<string, object> dict)
    {
        bool mainKeyPresent = _persistencyDict.ContainsKey(mainKeyString);
        if (!mainKeyPresent)
        {
            _persistencyDict.Add(mainKeyString, new Dictionary<string, object>());
        }
        var mainElement = _persistencyDict[mainKeyString];

        foreach (var keyValue in dict)
        {
            bool valueKeyPresent = mainElement.ContainsKey(keyValue.Key);
            if (valueKeyPresent)
            {
                mainElement.Remove(keyValue.Key);
            }
            mainElement.Add(keyValue.Key, keyValue.Value);
        }
        
        return true;
    }

    public bool Patch(
        string mainKey,
        Action<Dictionary<string, object>> action)
    {
        bool present = _persistencyDict.ContainsKey(mainKey);
        if (!present) { return false; }
        
        Dictionary<string, object> value = _persistencyDict[mainKey];
        action.Invoke(value);

        Save();
        return true;
    }
    
    public bool PutQSave(
        string mainKeyString,
        KeyValuePair<string, object> keyValue)
    {
        bool success = Put(mainKeyString, keyValue);
        if (success)
        {
            Save();
            return true;
        }
        
        return false;
    }

    public bool Put(
        string mainKeyString,
        KeyValuePair<string, object> keyValue)
    {
        bool mainKeyPresent = _persistencyDict.ContainsKey(mainKeyString);
        if (!mainKeyPresent) { return false; }
        Dictionary<string, object> mainElement = _persistencyDict[mainKeyString];
        bool valueKeyPresent = mainElement.ContainsKey(keyValue.Key);

        if (valueKeyPresent)
        {
            mainElement.Remove(keyValue.Key);
        }
        
        mainElement.Add(keyValue.Key, keyValue.Value);

        Save();
        return true;
    }

    private bool Load()
    {
        string bodyJson = _repoService.Item.GetItemBody(_cacheAdrTuple);
        var body = JsonConvert.DeserializeObject<object>(bodyJson).ToString();
        
        bool success = _fileService.Yaml.Custom03
            .TryDeserialize<List<Dictionary<string, object>>>(body, out var persistencyList);
        if (!success) {return false; }
        if (persistencyList is null)
        {
            _persistencyDict = new Dictionary<string, Dictionary<string, object>>();
            return true;
        }
        
        _persistencyDict = persistencyList
            .ToDictionary(x => x["key"].ToString(), x => x);
        return true;
    }

    public bool Save()
    {
        List<Dictionary<string, object>> persistencyList = _persistencyDict.Values.ToList();
        var text = _fileService.Yaml
            .Custom03.Serialize(persistencyList);
        _repoService.Methods.PutText(_cacheAdrTuple, "cache", text);
        return true;
    }
}
