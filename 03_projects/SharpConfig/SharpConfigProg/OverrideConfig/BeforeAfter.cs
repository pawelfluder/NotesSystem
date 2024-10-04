using SharpConfigProg.Service;
using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpConfigProg.OverrideConfig
{
    internal class BeforeAfter
    {
        private string configFolderName;
        private string overrideFileName;
        private string beforeFileName;
        private string afterFileName;

        private readonly IOperationsService operationsService;
        private readonly IConfigService configService;

        private string configFolderPath;
        private readonly IFileService _fileService;

        public BeforeAfter(
            IOperationsService operationsService,
            IConfigService configService)
        {
            configFolderName = "config";
            overrideFileName = "override.cfg";
            beforeFileName = "before.cfg";
            afterFileName = "after.cfg";

            this.operationsService = operationsService;
            this.configService = configService;
            _fileService = operationsService.GetFileService();
        }

        public void Run()
        {
            configFolderPath = TryGetSettingsFolder();

            if (configFolderPath == default)
            {
                return;
            }

            var anythingToOverride = AnythingToOverride();

            if (!anythingToOverride)
            {
                SaveAfter();
                return;
            }

            SaveBefore();
            OverrideSettings();
            SaveAfter();
        }

        private void SaveBefore()
        {
            var path = configFolderPath + "/" + beforeFileName;
            _fileService.Yaml.Custom03
                .SerializeToFile(path, configService.SettingsDict);
        }

        private void SaveAfter()
        {
            var path = configFolderPath + "/" + afterFileName;
            _fileService.Yaml.Custom03
                .SerializeToFile(path, configService.SettingsDict);
        }

        private bool AnythingToOverride()
        {
            if (File.Exists(configFolderPath + "/" + overrideFileName))
            {
                return true;
            }

            return false;
        }

        private void OverrideSettings()
        {
            try
            {
                var path = configFolderPath + "/" + overrideFileName;
                var dict = _fileService.Yaml.Custom03
                    .DeserializeFile<Dictionary<string, object>>(path);

                foreach (var kvp in dict)
                {
                    var success = configService.SettingsDict.TryGetValue(kvp.Key, out var value);

                    if (success)
                    {
                        configService.SettingsDict[kvp.Key] = kvp.Value;
                    }

                    configService.SettingsDict.Add(kvp.Key, kvp.Value);
                }
            }
            catch { }
        }

        private string TryGetSettingsFolder()
        {
            if (!Directory.Exists(configFolderName))
            {
                return default;
            }

            var path = Path.GetFullPath(configFolderName);
            return path;
        }
    }
}
