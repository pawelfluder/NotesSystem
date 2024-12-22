using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpConfigProg.OverrideConfig;

internal class BeforeAfter
{
    private string _overrideFileName;
    private string _beforeFileName;
    private string _afterFileName;

    private string? _settingsFolderPath;
    private readonly IFileService _fileService;
    private Dictionary<string,object> _settingsDict;

    public BeforeAfter(
        IOperationsService operationsService)
    {
        _overrideFileName = "overSettings.yaml";
        _beforeFileName = "beforeSettings.yaml";
        _afterFileName = "afterSettings.yaml";

        _fileService = operationsService.GetFileService();
    }

    public Dictionary<string, object> Run(
        Dictionary<string, object> settings)
    {
        _settingsDict = settings;
        if (!TryGetSettingsFolder())
        {
            return _settingsDict;
        }

        bool anythingToOverride = AnythingToOverride();

        if (!anythingToOverride)
        {
            SaveAfter();
            return _settingsDict;
        }

        SaveBefore();
        OverrideSettings();
        SaveAfter();
        return _settingsDict;
    }

    private void SaveBefore()
    {
        string path = _settingsFolderPath + "/" + _beforeFileName;
        _fileService.Yaml.Custom03
            .SerializeToFile(path, _settingsDict);
    }

    private void SaveAfter()
    {
        string path = _settingsFolderPath + "/" + _afterFileName;
        _fileService.Yaml.Custom03
            .SerializeToFile(path, _settingsDict);
    }

    private bool AnythingToOverride()
    {
        if (File.Exists(_settingsFolderPath + "/" + _overrideFileName))
        {
            return true;
        }

        return false;
    }

    private void OverrideSettings()
    {
        try
        {
            string overPath = _settingsFolderPath + "/" + _overrideFileName;
            Dictionary<string, object> overDict = _fileService.Yaml.Custom03
                .DeserializeFile<Dictionary<string, object>>(overPath);

            foreach (var overKvp in overDict)
            {
                bool success = _settingsDict
                    .TryGetValue(overKvp.Key, out object? value);

                if (!success)
                {
                    _settingsDict.Add(overKvp.Key, overKvp.Value);
                }

                if (success)
                {
                    _settingsDict[overKvp.Key] = overKvp.Value;
                }
            }
        }
        catch
        {}
    }

    private bool TryGetSettingsFolder()
    {
        _settingsFolderPath = _settingsDict["settingsFolderPath"]
            .ToString();
        if (string.IsNullOrEmpty(_settingsFolderPath))
        {
            return false;
        }

        return true;
    }
}
