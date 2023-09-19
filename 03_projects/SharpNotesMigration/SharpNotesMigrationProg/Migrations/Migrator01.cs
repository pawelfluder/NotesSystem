using SharpFileServiceProg.Service;
using SharpNotesMigrationProg.Service;
using SharpRepoServiceProg.Service;

namespace SharpNotesMigrationProg.Migrations
{
    internal class Migrator01 : IMigrator, IMigrationService.IMigrator01
    {
        private readonly IFileService fileService;
        private readonly IRepoService repoService;

        public Migrator01(
            IFileService fileService,
            IRepoService repoService)
        {
            this.fileService = fileService;
            this.repoService = repoService;
        }

        public void MigrateEverything()
        {
            var allRepos = repoService.Methods.GetAllReposNames();
            var allReposPath = repoService.Methods.GetAllReposPaths();
            var counts = new List<(string, int)>();
            foreach (var repo in allRepos)
            {
                var content = $"name: {repo}";
                repoService.Methods.CreateRepoConfig(repo, content);
            }
        }
    }
}
