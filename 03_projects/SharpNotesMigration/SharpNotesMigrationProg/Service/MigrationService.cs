using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpNotesMigrationProg.Migrations;
using SharpRepoServiceProg.Service;

namespace SharpNotesMigrationProg.Service
{
    public class MigrationService : IMigrationService
    {
        private readonly IFileService fileService;
        private readonly IConfigService configService;
        private readonly IRepoService repoService;

        public MigrationService(
            IFileService fileService,
            IRepoService repoService)
        {
            this.fileService = fileService;
            this.repoService = repoService;
            //this.configService = configService;
        }

        public void Migrate(Type migratorType)
        {
            IMigrator migrator = null;
            if (migratorType == typeof(IMigrationService.IMigrator03))
            {
                migrator = new Migrator03(fileService, repoService);
            }

            migrator.MigrateEverything();
        }

        public void MigrateAll()
        {
            var migrators = new List<IMigrator>
            {
                new Migrator03(fileService, repoService),
            };

            foreach (var migrator in migrators)
            {
                migrator.MigrateEverything();
            }
        }
    }
}
