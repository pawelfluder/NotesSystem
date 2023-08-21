using SharpConfigProg.Preparer;
using SharpFileServiceProg.Service;

namespace SharpConfigProg.Service
{
    internal class ConfigService : IConfigService
    {
        private readonly IFileService fileService;
        //private readonly RepoService repoService;
        private readonly IFileService.IYamlOperations yamlOperations;

        public string ConfigFilePath { get; }
        public Dictionary<string, object> SettingsDict
        {
            get;
            private set;
        }

        public ConfigService(
            IFileService fileService)
        {
            yamlOperations = fileService.Yaml.Custom03;
            var binPath = fileService.Path.GetBinPath();
            SettingsDict = new Dictionary<string, object>();
            ConfigFilePath = fileService.Path.MoveDirectoriesUp(binPath, 3) +
                "/" + "Config" + "/" + "paths.cfg";
            fileService.Path.CreateMissingDirectories(ConfigFilePath);

            this.fileService = fileService;
            //this.repoService = repoService;
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

        public void Prepare(Type preparerClassType)
        {
            if (preparerClassType == typeof(IPreparer.IOnlyRootPaths))
            {
                SetAndSerializePaths(new OnlyRootPathsPreparer()
                    .Prepare());
            }

            if (preparerClassType == typeof(IPreparer.IWinder))
            {
                SetAndSerializePaths(new WinderPreparer(ConfigFilePath)
                    .Prepare());
            }

            if (preparerClassType == typeof(IPreparer.ILocalProgramData))
            {
                SetAndSerializePaths(new LocalProgramDataPreparer(fileService)
                    .Prepare());
            }

            if (preparerClassType == typeof(IPreparer.INotesSystem))
            {
                SetAndSerializePaths(new NotesSystemPreparer(fileService)
                    .Prepare());
            }
        }

        public void AddSetting(string key, object value)
        {
            SettingsDict.Add(key, value);
        }
    }
}
