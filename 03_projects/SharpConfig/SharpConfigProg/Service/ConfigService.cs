using SharpConfigProg.ConfigPreparer;
using SharpConfigProg.Repetition;
using SharpFileServiceProg.Service;

namespace SharpConfigProg.Service
{
    internal partial class ConfigService : IConfigService
    {
        private readonly IFileService.IYamlOperations yamlOperations;

        public IFileService fileService;

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
            IFileService fileService)
        {
            SettingsDict = new Dictionary<string, object>();
            this.fileService = fileService;
            yamlOperations = this.fileService.Yaml.Custom03;

            CreateConfigFile();
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

        protected void SetAndSerializePaths(Dictionary<string, object> paths)
        {
            SettingsDict = paths;
            SettingsDict.ToList().ForEach(x => Console.WriteLine(x));
            yamlOperations.SerializeToFile(ConfigFilePath, SettingsDict);
        }

        public void Prepare()
        {
            var preparer = MyBorder.Container.Resolve<IPreparer>();
            var paths = preparer.Prepare();
            SetAndSerializePaths(paths);
        }

        public void Prepare(Type preparerClassType)
        {
            var preparer = MyBorder.Container.Resolve(preparerClassType);
            var paths = (preparer as IPreparer).Prepare();
            SetAndSerializePaths(paths);
        }

        public void AddSetting(string key, object value)
        {
            SettingsDict.Add(key, value);
        }

        private void CreateConfigFile()
        {
#if DEBUG
            var binPath = this.fileService.Path.TryGetBinPath(out var success);
            if (success)
            {
                ConfigFilePath = this.fileService.Path.MoveDirectoriesUp(binPath, 3) +
                "/" + "Config" + "/" + "paths.cfg";
                this.fileService.Path.CreateMissingDirectories(ConfigFilePath);
            }
#endif
        }
    }
}
