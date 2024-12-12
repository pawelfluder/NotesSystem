using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Names;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;

namespace SharpRepoServiceProg.Workers.System;

internal class ConfigWorker
{
    private readonly IFileService _fileService;
    private readonly IYamlOperations _yamlOperations;
    private readonly PathWorker _pw;
    private readonly SystemWorker _sw;
    private readonly CustomOperationsService _customOperationsService;
    public object ErrorValue { get; internal set; }

    public ConfigWorker()
    {
        _fileService = MyBorder.OutContainer.Resolve<IFileService>();
        _customOperationsService = MyBorder.MyContainer.Resolve<CustomOperationsService>();
        _yamlOperations = _fileService.Yaml.Custom03;
        _pw = MyBorder.MyContainer.Resolve<PathWorker>();
        _sw = MyBorder.MyContainer.Resolve<SystemWorker>();
    }

    public bool TryGetConfigLines(
        (string Repo, string Loca) address,
        out List<string> lines)
    {
        try
        {
            lines = GetConfigLines(address);
            return true;
        }
        catch
        {
            lines = null;
            return false;
        }
    }

    public string GetConfigText(
        (string Repo, string Loca) adrTuple)
    {
        string configFilePath = _pw.GetConfigPath(adrTuple);
        string[] configLines = File.ReadAllLines(configFilePath);
        string configText = string.Join(_sw.NewLine, configLines);
        return configText;
    }

    public List<string> GetConfigLines(
        (string Repo, string Loca) adrTuple)
    {
        string configFilePath = _pw.GetConfigPath(adrTuple);
        List<string> configLines = File.ReadAllLines(configFilePath).ToList();
        return configLines;
    }

    public void AddSettingsToModel(
        ItemModel model,
        (string Repo, string Loca) adrTuple,
        Dictionary<string, object> settings)
    {
        string newAddress = _customOperationsService.UniAddress.CreateUrlFromAddress(adrTuple);
        settings[FieldsForUniItem.Address] = newAddress;
        bool saveNeeded = false;
        if (!settings.ContainsKey(FieldsForUniItem.Id))
        {
            settings[FieldsForUniItem.Id] = Guid.NewGuid().ToString();
            saveNeeded = true;
        }
        if (!settings.ContainsKey(FieldsForUniItem.Type))
        {
            var type = AssumeType(adrTuple);
            settings[FieldsForUniItem.Type] = type;
            saveNeeded = true;
        }
        model.Settings = settings;

        if (saveNeeded)
        {
            CreateConfig(adrTuple, settings);
        }
    }

    public string AssumeType(
        (string repo, string loca) adrTuple)
    {
        var contentFilePath = _pw.GetBodyPath(adrTuple);
        if (File.Exists(contentFilePath))
        {
            return UniItemTypes.Text;
        }

        return UniItemTypes.Folder;
    }

    public void CreateConfig(
        (string Repo, string Loca) adrTuple,
        Dictionary<string, object> dict)
    {
        var nameFilePath = _pw.GetConfigPath(adrTuple);
        var content = _yamlOperations.Serialize(dict);
        File.WriteAllText(nameFilePath, content);
    }

    public void CreateConfig(
        (string Repo, string Loca) adrTuple,
        List<string> contentLines)
    {
        var nameFilePath = _pw.GetConfigPath(adrTuple);
        var content = string.Join(_sw.NewLine, contentLines);
        File.WriteAllText(nameFilePath, content);
    }

    public Dictionary<string, object> GetConfigDictionary(
        (string Repo, string Loca) adrTuple)
    {
        var configItemPath = _pw.GetConfigPath(adrTuple);
        var dict = _yamlOperations.DeserializeFile<Dictionary<string, object>>(configItemPath);
        return dict;
    }

    public void CreateConfigKey(
        (string Repo, string Loca) address,
        string key,
        object value)
    {
        var dict = GetConfigDictionary(address);
        var exists = dict.TryGetValue(key, out var tmp);
        if (exists)
        {
            dict[key] = value;
        }

        if (!exists)
        {
            dict.Add(key, value);
            try
            {
                CreateConfig(address, dict);
            }
            catch { };
        }
    }



    //public void CreateConfig(
    //    string itemPath,
    //    Dictionary<string, object> dict)
    //{
    //    var nameFilePath = itemPath + slash + configFileName;
    //    var content = yamlOperations.Serialize(dict);
    //    File.WriteAllText(nameFilePath, content);
    //}
}