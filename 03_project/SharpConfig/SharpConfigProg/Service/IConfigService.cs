namespace SharpConfigProg.Service
{
    public interface IConfigService
    {
        string ConfigFilePath { get; }
        Dictionary<string, object> SettingsDict { get; }

        void Prepare(Type preparationClassType);
        void LoadSettingsFromFile();
        List<string> GetRepoSearchPaths();
    }
}