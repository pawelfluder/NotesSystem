using RepoServiceCoreProj.Service;
using SharpConfigProg.Service;
using SharpFileServiceProg.Yaml;
using SharpNotesExporter;

namespace SharpTinderComplexTests
{
    public class UnitTest01Base
    {
        protected readonly YamlWorker yamlWorker;
        protected readonly RepoService repoService;
        protected readonly ConfigService configService;

        protected readonly NotesExporterService notesExporterService;

        protected readonly string configFilePath;
        protected Dictionary<string, object> ConfigData { get; private set; }

        protected UnitTest01Base()
        {

            configService = new ConfigService();
            configFilePath = configService.ConfigFilePath;
            yamlWorker = new YamlWorker();
            LoadConfigData();
            var repoRootPaths = GetRepoRootPaths();
            repoService = new RepoService(repoRootPaths);
            notesExporterService = new NotesExporterService(repoService);
        }

        protected void LoadConfigData()
        {
            if (File.Exists(configFilePath))
            {
                ConfigData = yamlWorker.SharpDeserializeFile<Dictionary<string, object>>(configFilePath);
            }
        }

        protected List<string> GetRepoRootPaths()
        {
            var repoRootPaths = new List<string>();
            if (File.Exists(configFilePath) &&
                ConfigData.TryGetValue("repoRootPaths", out var tmp))
            {
                var tmp2 = ((List<object>)tmp);
                repoRootPaths = tmp2.Select(x => x.ToString()).ToList();
            }
            return repoRootPaths;
        }
    }
}