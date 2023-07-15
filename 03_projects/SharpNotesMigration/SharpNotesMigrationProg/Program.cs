using SharpConfigProg.Preparer;
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
            configService.Prepare(typeof(IPreparer.IOnlyRootPaths));
            var repoService = new RepoService(fileService, configService.GetRepoSearchPaths());
            var migrationService = new MigrationService(fileService, repoService);
            migrationService.MigrateAll();
        }
    }
}