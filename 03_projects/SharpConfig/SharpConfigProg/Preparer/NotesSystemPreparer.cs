using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Repetition;

namespace SharpConfigProg.Preparer
{
    internal class NotesSystemPreparer : IPreparer
    {
        private readonly IFileService fileService;

        public NotesSystemPreparer(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public Dictionary<string, object> Prepare()
        {
            var repoRootPaths = GetRepoSearchPaths();
            var credentialWorker = new CredentialWorker();
            (var googleClientId,var googleClientSecret) = credentialWorker.GetCredentials();

            var paths = new Dictionary<string, object>()
            {
                { nameof(repoRootPaths), repoRootPaths },
                { nameof(googleClientId), googleClientId },
                { nameof(googleClientSecret), googleClientSecret },
            };

            paths.ToList().ForEach(x => Console.WriteLine(x.Key + ": " + x.Value));
            return paths;
        }

        public List<object> GetRepoSearchPaths()
        {
            var synchFolderPath = "D:/01_Synchronized/01_Programming_Files";
            var tmp = Directory.GetDirectories(synchFolderPath);
            var tmp3 = tmp.Where(x => Guid.TryParse(Path.GetFileName(x), out var tmp2));
            var repoSearchPaths = tmp3.Select(x => (object)x).ToList();
            return repoSearchPaths;
        }
    }
}
