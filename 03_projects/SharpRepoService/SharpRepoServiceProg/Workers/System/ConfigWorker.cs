﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registrations;

namespace SharpRepoServiceProg.Workers.System;

internal class ConfigWorker
{
    private readonly IFileService _fileService;
    private readonly IYamlOperations _yamlOperations;
    private readonly PathWorker _path;
    private readonly SystemWorker _system;
    private readonly CustomOperationsService _customOperationsService;
    public object ErrorValue { get; internal set; }

    public ConfigWorker()
    {
        _fileService = MyBorder.OutContainer.Resolve<IFileService>();
        _customOperationsService = MyBorder.MyContainer.Resolve<CustomOperationsService>();
        _yamlOperations = _fileService.Yaml.Custom03;
        _path = MyBorder.MyContainer.Resolve<PathWorker>();
        _system = MyBorder.MyContainer.Resolve<SystemWorker>();
    }

    // public bool TryGetConfigLines(
    //     (string Repo, string Loca) address,
    //     out List<string> lines)
    // {
    //     try
    //     {
    //         lines = GetConfigLines(address);
    //         return true;
    //     }
    //     catch
    //     {
    //         lines = null;
    //         return false;
    //     }
    // }

    public string GetConfigText(
        (string Repo, string Loca) adrTuple)
    {
        string configFilePath = _path.GetConfigPath(adrTuple);
        string[] configLines = File.ReadAllLines(configFilePath);
        string configText = string.Join(_system.NewLine, configLines);
        return configText;
    }

    public List<string> GetConfigLines(
        (string Repo, string Loca) adrTuple)
    {
        string configFilePath = _path.GetConfigPath(adrTuple);
        List<string> configLines = File.ReadAllLines(configFilePath).ToList();
        return configLines;
    }

    public void PutConfig(
        (string Repo, string Loca) adrTuple,
        Dictionary<string, object> dict)
    {
        var nameFilePath = _path.GetConfigPath(adrTuple);
        var content = _yamlOperations.Serialize(dict);
        File.WriteAllText(nameFilePath, content);
    }
    
    // public void PatchConfig(
    //     (string Repo, string Loca) adrTuple,
    //     KeyValuePair<string, object> keyValue)
    // {
    //     var nameFilePath = _path.GetConfigPath(adrTuple);
    //     var content = _yamlOperations.Serialize(dict);
    //     File.WriteAllText(nameFilePath, content);
    // }

    public void PutConfig(
        (string Repo, string Loca) adrTuple,
        List<string> contentLines)
    {
        string nameFilePath = _path.GetConfigPath(adrTuple);
        string content = string.Join(_system.NewLine, contentLines);
        File.WriteAllText(nameFilePath, content);
    }

    public string GetType(
        (string Repo, string Loca) adrTuple)
    {
        Dictionary<string, object> dict = GetConfigDictionary(adrTuple);
        bool success = dict.TryGetValue(ConfigKeys.Type, out var type);
        return type?.ToString();
    }
    
    public bool ItemExists(
        (string Repo, string Loca) adrTuple,
        out string type)
    {
        type = GetType(adrTuple);
        if (string.IsNullOrEmpty(type))
        {
            return false;
        }

        return true;
    }

    public Dictionary<string, object> GetConfigDictionary(
        (string Repo, string Loca) adrTuple)
    {
        string configItemPath = _path.GetConfigPath(adrTuple);
        Dictionary<string, object> dict = _yamlOperations
            .DeserializeFile<Dictionary<string, object>>(configItemPath);
        return dict ?? new Dictionary<string, object>();
    }

    public void CreateConfigKey(
        (string Repo, string Loca) address,
        string key,
        object value)
    {
        Dictionary<string, object> dict = GetConfigDictionary(address);
        bool exists = dict.TryGetValue(key, out var tmp);
        if (exists)
        {
            dict[key] = value;
        }

        if (!exists)
        {
            dict.Add(key, value);
            try
            {
                PutConfig(address, dict);
            }
            catch { };
        }
    }
}
