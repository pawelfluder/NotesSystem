using SharpFileServiceProg.Service;
using SharpNotesMigrationProg.AAPublic;
using SharpNotesMigrationProg.Service;
using SharpRepoServiceProg.AAPublic;
using Unity;
using OutBorder1 = SharpFileServiceProg.AAPublic.OutBorder;
using OutBorder2 = SharpConfigProg.AAPublic.OutBorder;
using OutBorder3 = SharpRepoServiceProg.AAPublic.OutBorder;
using OutBorder4 = SharpNotesMigrationProg.Repetition.OutBorder;

namespace SharpNotesMigrationTests.Repetition
{
    internal class Registration : RegistrationBase
    {
        protected override void Registrations()
        {
            RegisterByFunc(OutBorder1.FileService);

            RegisterByFunc(
                OutBorder2.ConfigService,
                container.Resolve<IFileService>());

            RegisterByFunc<IRepoService, IFileService>(OutBorder3.RepoService,
                container.Resolve<IFileService>());

            RegisterByFunc<IMigrationService, IFileService, IRepoService>
                (OutBorder4.MigrationService,
                container.Resolve<IFileService>(),
                container.Resolve<IRepoService>());

            RegisterByFunc<IMigrator03, IFileService, IRepoService>
                (OutBorder4.Migrator03,
                container.Resolve<IFileService>(),
                container.Resolve<IRepoService>());
        }
    }
}
