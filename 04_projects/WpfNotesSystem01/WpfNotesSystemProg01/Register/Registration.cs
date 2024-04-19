using SharpContainerProg.AAPublic;

using OutBorder1 = SharpFileServiceProg.AAPublic.OutBorder;
using OutBorder2 = SharpConfigProg.AAPublic.OutBorder;
using OutBorder3 = SharpRepoServiceProg.AAPublic.OutBorder;
using OutBorder4 = SharpGoogleDriveProg.AAPublic.OutBorder;
using OutBorder5 = SharpGoogleDocsProg.AAPublic.OutBorder;

namespace WpfNotesSystemProg01.Register
{
    public class Registration : RegistrationBase
    {
        public override void Registrations()
        {
            var fileService = OutBorder1.FileService();
            RegisterByFunc(() => fileService);

            var configService = OutBorder2.ConfigService(fileService);
            RegisterByFunc(() => configService);

            var repoService = OutBorder3.RepoService(fileService);
            RegisterByFunc(() => repoService);

            configService.Prepare();
            repoService.Initialize(configService.GetRepoSearchPaths());

            var driveService = OutBorder4.GoogleDriveService(
                configService.SettingsDict);
            RegisterByFunc(() => driveService);

            var docsService = OutBorder5.GoogleDocsService(
                configService.SettingsDict);
            RegisterByFunc(() => docsService);
        }
    }
}
