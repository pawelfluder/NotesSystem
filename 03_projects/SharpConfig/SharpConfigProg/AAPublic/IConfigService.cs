﻿namespace SharpConfigProg.AAPublic;

public partial interface IConfigService
{
    string ConfigFilePath { get; }
    Dictionary<string, object> SettingsDict { get; }
    bool TryGetSettingAsString(string key, out string value);

    void AddSetting(string key, object value);
    void OverrideSetting(string key, object value);
    void Prepare(Type preparationClassType);
    void Prepare(Dictionary<string, object> settingsDict);
    void Prepare(IPreparer preparer);
    void Prepare();
    void LoadSettingsFromFile();
    List<string> GetRepoSearchPaths();
}