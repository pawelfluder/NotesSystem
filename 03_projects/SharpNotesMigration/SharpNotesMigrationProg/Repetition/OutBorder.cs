using SharpFileServiceProg.Service;
using SharpNotesMigrationProg.AAPublic;
using SharpNotesMigrationProg.Migrations;
using SharpNotesMigrationProg.Service;
using SharpRepoServiceProg.AAPublic;

namespace SharpNotesMigrationProg.Repetition
{
    public class OutBorder
    {
        public static IMigrationService MigrationService(
            IFileService fileService,
            IRepoService repoService)
        {
            return new MigrationService(fileService, repoService);
        }

        public static IMigrator03 Migrator03(
            IFileService fileService,
            IRepoService repoService)
        {
            return new Migrator03(fileService, repoService);
        }
    }
}
