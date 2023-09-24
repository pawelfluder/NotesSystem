namespace SharpConfigProg.Service
{
    public partial interface IConfigService
    {
        string ConfigFilePath { get; }
        Dictionary<string, object> SettingsDict { get; }

        void AddSetting(string key, object value);
        void Prepare(Type preparationClassType);
        void LoadSettingsFromFile();
        List<string> GetRepoSearchPaths();
    }
}