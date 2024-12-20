﻿using SharpConfigProg.AAPublic;
using SharpConfigProg.OverrideConfig;
using SharpConfigProg.Registrations;
using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpConfigProg.Service;

internal partial class ConfigService : IConfigService
{
    private readonly IYamlOperations yamlOperations;

    public IOperationsService _operationsService;
    private readonly IFileService _fileService;

    public string ConfigFilePath { get; private set; }
    public Dictionary<string, object> SettingsDict
    {
        get;
        private set;
    }

    public bool TryGetSettingAsString(string key, out string value)
    {
        var success = SettingsDict.TryGetValue(key, out var valueObj);
        if (success)
        {
            value = valueObj.ToString();
            return success;
        }
        value = null;
        return false;
    }

    public ConfigService(
        IOperationsService operationsService)
    {
        _fileService = operationsService.GetFileService();
        SettingsDict = new Dictionary<string, object>();
        this._operationsService = operationsService;
        yamlOperations = _fileService.Yaml.Custom03;
    }

    public List<string> GetRepoSearchPaths()
    {
        var repoRootPaths = (SettingsDict["repoRootPaths"] as List<object>)
            .Select(x => x.ToString()).ToList();
        return repoRootPaths;
    }

    public void LoadSettingsFromFile()
    {
        var tmp = yamlOperations.DeserializeFile<Dictionary<string, object>>(ConfigFilePath);
        SettingsDict = tmp;
    }


    public void Prepare()
    {
        var preparer = MyBorder.OutContainer.Resolve<IPreparer>();
        preparer.SetConfigService(this);
        var settings = preparer.Prepare();
        SettingsDict = settings;
        new BeforeAfter(_operationsService, this).Run();
    }

    public void Prepare(IPreparer preparer)
    {
        preparer.SetConfigService(this);
        SettingsDict = preparer.Prepare(); ;
        new BeforeAfter(_operationsService, this).Run();
    }

    public void Prepare(Dictionary<string, object> dict)
    {
        SettingsDict = new Dictionary<string, object>(dict);
        new BeforeAfter(_operationsService, this).Run();
    }

    public void Prepare(Type preparerClassType)
    {
        var tmp = MyBorder.OutContainer.Resolve(preparerClassType);
        var preparer = (tmp as IPreparer);
        preparer.SetConfigService(this);
        SettingsDict = preparer.Prepare(); ;
        new BeforeAfter(_operationsService, this).Run();
    }

    public void AddSetting(string key, object value)
    {
        SettingsDict.Add(key, value);
    }

    public void OverrideSetting(string key, object value)
    {
        var success = SettingsDict.TryGetValue(key, out var tmp);
        if (!success)
        {
            AddSetting(key, value);
        }

        SettingsDict[key] = value;
    }
}