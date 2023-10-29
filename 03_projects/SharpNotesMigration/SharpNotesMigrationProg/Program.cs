using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpNotesMigrationProg.Service;
using SharpRepoBackendProg.Repetition;
using SharpRepoServiceProg.Service;
using Unity;

namespace SharpNotesMigrationProg
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fileService = MyBorder.Container.Resolve<IFileService>();
            var configService = MyBorder.Container.Resolve<IConfigService>();
            configService.Prepare(typeof(IConfigService.ILocalProgramDataPreparer));
            var repoService = MyBorder.Container.Resolve<IRepoService>();
            repoService.Initialize(configService.GetRepoSearchPaths());
            var migrationService = new MigrationService(fileService, repoService);
            migrationService.MigrateAll();
        }
    }
}