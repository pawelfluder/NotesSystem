using SharpConfigProg.Preparer;
using SharpConfigProg.Repetition;
using SharpFileServiceProg.Service;
using Unity;

namespace SharpConfigProg.Service
{
    internal class ConfigService : IConfigService
    {
        //private readonly RepoService repoService;
        private readonly IFileService.IYamlOperations yamlOperations;

        public IFileService fileService;

        public string ConfigFilePath { get; }
        public Dictionary<string, object> SettingsDict
        {
            get;
            private set;
        }

        public ConfigService(
            IFileService fileService)
        {
            this.fileService = fileService;
            yamlOperations = this.fileService.Yaml.Custom03;
            var binPath = this.fileService.Path.GetBinPath();
            SettingsDict = new Dictionary<string, object>();
            ConfigFilePath = this.fileService.Path.MoveDirectoriesUp(binPath, 3) +
                "/" + "Config" + "/" + "paths.cfg";
            this.fileService.Path.CreateMissingDirectories(ConfigFilePath);
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
            var preparer = MyBorder.Container.Resolve(preparerClassType);
            var paths = (preparer as IPreparer).Prepare();
            SetAndSerializePaths(paths);

            //f (preparerClassType == typeof(IPreparer.IOnlyRootPathsPreparer))
            //{
            //    SetAndSerializePaths(new iOnlyRootPathsPreparer()
            //        .Prepare());
            //}

            //if (preparerClassType == typeof(IPreparer.IWinder))
            //{
            //    SetAndSerializePaths(new WinderPreparer(ConfigFilePath)
            //        .Prepare());
            //}

            //if (preparerClassType == typeof(IPreparer.ILocalProgramData))
            //{
            //    SetAndSerializePaths(new LocalProgramDataPreparer(fileService)
            //        .Prepare());
            //}

            //if (preparerClassType == typeof(IPreparer.INotesSystem))
            //{
            //    SetAndSerializePaths(new NotesSystemPreparer(fileService)
            //        .Prepare());
            //}
        }

        public void AddSetting(string key, object value)
        {
            SettingsDict.Add(key, value);
        }
    }
}
