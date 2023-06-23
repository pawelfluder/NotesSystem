using SharpFileServiceProg.Service;
using System.IO;

namespace SharpConfigProg.Preparer
{
    internal class LocalProgramDataPreparer : IPreparer, IPreparer.ILocalProgramData
    {
        private readonly IFileService fileService;

        public LocalProgramDataPreparer(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public Dictionary<string, object> Prepare()
        {
            var paths = new Dictionary<string, object>();

            var repoRootPaths = GetRepoPaths2();
            paths = new Dictionary<string, object>()
            {
                { "repoRootPaths", repoRootPaths},
            };
            paths.ToList().ForEach(x => Console.WriteLine(x.Key + ": " + x.Value));

            return paths;
        }

        public List<object> GetRepoPaths2()
        {
            var startupPath = Directory.GetCurrentDirectory();
            var startupPath2 = Environment.CurrentDirectory;
            var tmpPath = fileService.Path.MoveDirectoriesUp(startupPath, 3);
            var programDataFolderPath = tmpPath + "/" + "02_program-data";
            var searchPaths = new List<object> { programDataFolderPath };
            return searchPaths;
        }
    }
}
