using SharpConfigProg.APublic;
using SharpConfigProg.ConfigPreparer;

namespace SharpConfigProg.Service
{
    public partial interface IConfigService
    {
        string ConfigFilePath { get; }
        Dictionary<string, object> SettingsDict { get; }
        bool TryGetSettingAsString(string key, out string value);

        void AddSetting(string key, object value);
        void Prepare(Type preparationClassType);
        void Prepare(IPreparer preparer);
        void Prepare();
        void LoadSettingsFromFile();
        List<string> GetRepoSearchPaths();
    }
}