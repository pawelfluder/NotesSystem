using SharpNotesMigrationProg.AAPublic;
using SharpNotesMigrationProg.Migrations;
using SharpNotesMigrationProg.Service;
using SharpOperationsProg.AAPublic.Operations;
using SharpRepoServiceProg.AAPublic;

namespace SharpNotesMigrationProg.Repetition
{
    public class OutBorder
    {
        public static IMigrationService MigrationService(
            IOperationsService operationsService,
            IRepoService repoService)
        {
            return new MigrationService(operationsService, repoService);
        }

        public static IMigrator03 Migrator03(
            IOperationsService operationsService,
            IRepoService repoService)
        {
            return new Migrator03(operationsService, repoService);
        }
    }
}
