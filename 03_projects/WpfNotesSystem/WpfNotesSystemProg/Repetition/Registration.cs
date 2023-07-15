using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using Unity;
using OutBorder1 = SharpFileServiceProg.Repetition.OutBorder;
using OutBorder2 = SharpConfigProg.Repetition.OutBorder;

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
        }

        //protected override void Registrations()
        //{
        //    RegisterByFunc(OutBorder1.FileService);

        //    RegisterByFunc(
        //        OutBorder2.NewConfigService,
        //        container.Resolve<IFileService>());

        //    RegisterByFunc<RepoService, IFileService>(OutBorder3.RepoService,
        //        container.Resolve<IFileService>());

        //    RegisterByFunc<IMigrationService, IFileService, RepoService>
        //        (OutBorder4.MigrationService,
        //        container.Resolve<IFileService>(),
        //        container.Resolve<RepoService>());

        //    RegisterByFunc<IMigrationService.IMigrator03, IFileService, RepoService>
        //        (OutBorder4.Migrator03,
        //        container.Resolve<IFileService>(),
        //        container.Resolve<RepoService>());
        //}
    }
}
