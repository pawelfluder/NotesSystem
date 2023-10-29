using SharpConfigProg.AAPublic;
using SharpConfigProg.OverrideConfig;
using SharpConfigProg.Register;
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
            var preparer = MyBorder.Container.Resolve<IPreparer>();
            preparer.SetConfigService(this);
            var settings = preparer.Prepare();
            SettingsDict = settings;
            new BeforeAfter(fileService, this).Run();
        }

        public void Prepare(IPreparer preparer)
        {
            var settings = preparer.Prepare();
            preparer.SetConfigService(this);
            SettingsDict = settings;
            new BeforeAfter(fileService, this).Run();
        }

        public void Prepare(Type preparerClassType)
        {
            var tmp = MyBorder.Container.Resolve(preparerClassType);
            var preparer = (tmp as IPreparer);
            preparer.SetConfigService(this);
            SettingsDict = preparer.Prepare(); ;
            new BeforeAfter(fileService, this).Run();
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
}
