using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpNotesMigrationProg.Migrations;
using SharpRepoServiceProg.AAPublic;

namespace SharpNotesMigrationProg.Service
{
    public class MigrationService : IMigrationService
    {
        private readonly IFileService fileService;
        private readonly IConfigService configService;
        private readonly IRepoService repoService;

        private List<IMigrator> migratorsList;

        public MigrationService(
            IFileService fileService,
            IRepoService repoService)
        {
            this.fileService = fileService;
            this.repoService = repoService;
            //this.configService = configService;

            migratorsList = new List<IMigrator>()
            {
                new Migrator03(fileService, repoService),
                new Migrator04(fileService, repoService),
            };
        }

        public void MigrateOneAddress(
            Type migratorType,
            (string Repo, string Loca) address)
        {
            var found = migratorsList.SingleOrDefault(x => x.GetType() == migratorType);

            if (found != null)
            {
                found.MigrateOneAddress(address);
            }
        }

        public void MigrateOneFolder(
            Type migratorType,
            (string Repo, string Loca) address,
            bool agree)
        {
            var found = migratorsList.SingleOrDefault(x => x.GetType().GetInterfaces().Contains(migratorType));

            if (found != null)
            {
                found.SetAgree(agree);
                found.MigrateOneFolder(address);
            }            
        }

        public void MigrateOneRepo(
            Type migratorType,
            string repoName)
        {
            var found = migratorsList.SingleOrDefault(x => x.GetType() == migratorType);

            if (found != null)
            {
                found.MigrateOneRepo(repoName);
            }
        }

        public void MigrateAllRepos(Type migratorType)
        {
            var found = migratorsList.SingleOrDefault(x => x.GetType() == migratorType);

            if (found != null)
            {
                found.MigrateAllRepos();
            }
        }
    }
}
