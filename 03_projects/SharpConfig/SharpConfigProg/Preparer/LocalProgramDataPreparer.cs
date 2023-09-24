using SharpFileServiceProg.Service;

namespace SharpConfigProg.Preparer
{
    internal class LocalProgramDataPreparer : IPreparer.ILocalProgramData
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
            var startupPath = Directory.GetCurrentDirectory();
            var startupPath2 = Environment.CurrentDirectory;
            var tmpPath = fileService.Path.MoveDirectoriesUp(startupPath, 6);
            var programDataFolderPath = tmpPath + "/" + "17_projects/02_program-data";
            var searchPaths = new List<object> { programDataFolderPath };
            return searchPaths;
        }
    }
}
