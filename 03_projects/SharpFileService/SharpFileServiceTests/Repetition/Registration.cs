using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Service;
using Unity;
using OutBorder1 = SharpFileServiceProg.AAPublic.OutBorder;
using OutBorder2 = SharpConfigProg.AAPublic.OutBorder;
using OutBorder3 = SharpRepoServiceProg.AAPublic.OutBorder;

namespace SharpNotesMigrationTests.Repetition
{
    internal class Registration : RegistrationBase
    {
        protected override void Registrations()
        {
            RegisterByFunc<IFileService>(OutBorder1.FileService);

            RegisterByFunc<IConfigService, IFileService>(
                OutBorder2.ConfigService,
                container.Resolve<IFileService>());

            RegisterByFunc<IRepoService, IFileService>(
                OutBorder3.RepoService,
                container.Resolve<IFileService>());

            var configService = container.Resolve<IConfigService>();
            var repoService = container.Resolve<IRepoService>();
            configService.Prepare(typeof(IConfigService.ILocalProgramDataPreparer));
            repoService.Initialize(configService.GetRepoSearchPaths());
        }
    }
}
