using SharpFileServiceProg.Service;
using SharpNotesMigrationProg.Service;
using SharpRepoServiceProg.AAPublic;

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

        public void MigrateAllRepos()
        {
            throw new NotImplementedException();
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

        public void MigrateOneAddress((string Repo, string Loca) address)
        {
            throw new NotImplementedException();
        }

        public void MigrateOneFolder((string Repo, string Loca) adrTuple)
        {
            throw new NotImplementedException();
        }

        public void MigrateOneRepo(string repoName)
        {
            throw new NotImplementedException();
        }

        public void SetAgree(bool agree)
        {
            throw new NotImplementedException();
        }
    }
}
