using SharpConfigProg.Preparer;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpNotesExporter;
using SharpNotesExporterTests.Repetition;
using SharpRepoServiceProg.Service;
using Unity;

namespace SharpTinderComplexTests
{
    public class UnitTest01Base
    {
        protected readonly IFileService.IYamlOperations yamlWorker;
        protected readonly IRepoService repoService;
        private readonly IFileService fileService;
        protected readonly IConfigService configService;

        protected readonly NotesExporterService notesExporterService;

        protected readonly string configFilePath;
        protected Dictionary<string, object> ConfigData { get; private set; }

        protected UnitTest01Base()
        {
            fileService = MyBorder.Container.Resolve<IFileService>();
            configService = MyBorder.Container.Resolve<IConfigService>();
            configService.Prepare(typeof(IPreparer.INotesSystem));
            configFilePath = configService.ConfigFilePath;
            yamlWorker = fileService.Yaml.Custom03;
            //LoadConfigData();
            //var repoRootPaths = GetRepoRootPaths();
            repoService = MyBorder.Container.Resolve<RepoService>();
            repoService.Methods.InitializeSearchFoldersPaths(configService.GetRepoSearchPaths());
            notesExporterService = new NotesExporterService(repoService);
        }

        //protected void LoadConfigData()
        //{
        //    if (File.Exists(configFilePath))
        //    {
        //        ConfigData = yamlWorker.Deserialize<Dictionary<string, object>>(configFilePath);
        //    }
        //}

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