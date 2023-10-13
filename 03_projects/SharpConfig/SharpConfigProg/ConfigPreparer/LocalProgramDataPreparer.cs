using SharpConfigProg.ConfigPreparer;
using SharpFileServiceProg.Service;

namespace SharpConfigProg.Service
{
    public partial interface IConfigService
    {
        public interface ILocalProgramDataPreparer : IPreparer { }
    }

    public class LocalProgramDataPreparer : IConfigService.ILocalProgramDataPreparer
    {
        private readonly IFileService fileService;

        public LocalProgramDataPreparer(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public Dictionary<string, object> Prepare()
        {
            var paths = new Dictionary<string, object>();

            var repoRootPaths = GetRepoSearchPaths2();
            paths = new Dictionary<string, object>()
            {
                { "repoRootPaths", repoRootPaths},
            };
            paths.ToList().ForEach(x => Console.WriteLine(x.Key + ": " + x.Value));

            return paths;
        }

        public List<object> GetRepoSearchPaths2()
        {
            var programDataFolderPath = fileService.Path.GetProjectFolderPath("18296f12-0706-43e1-9bd4-1b40154ec22e");
            var searchPaths = new List<object> { programDataFolderPath };
            return searchPaths;
        }
    }
}
