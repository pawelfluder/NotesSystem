using SharpContainerProg.AAPublic;
using SharpFileServiceProg.Service;
using OutBorder1 = SharpFileServiceProg.AAPublic.OutBorder;
using OutBorder2 = SharpConfigProg.AAPublic.OutBorder;
using OutBorder3 = SharpRepoServiceProg.AAPublic.OutBorder;
using OutBorder4 = SharpGoogleDriveProg.AAPublic.OutBorder;
using OutBorder5 = SharpGoogleDocsProg.AAPublic.OutBorder;
using OutBorder6 = SharpGoogleSheetProg.AAPublic.OutBorder;
using OutBorder7 = SharpTtsServiceProg.AAPublic.OutBorder;
using OutBorder8 = SharpVideoServiceProg.AAPublic.OutBorder;

namespace SharpSetup21ProgPrivate.Registrations
{
    public class DefaultRegistration : RegistrationBase
    {
        private Dictionary<string, object> tempDict;
        private IFileService fileService;

        public void SetSettingsDict(Dictionary<string, object> inputDict)
        {
            this.tempDict = inputDict;
        }

        public void SetFileService(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public override void Registrations()
        {
            RegisterByFunc(() => this.fileService);

            var configService = OutBorder2.ConfigService(fileService);
            RegisterByFunc(() => configService);

            var repoService = OutBorder3.RepoService(fileService);
            RegisterByFunc(() => repoService);

            configService.Prepare(tempDict);
            var searchPaths = configService.GetRepoSearchPaths();
            repoService.PutPaths(searchPaths);

            var driveService = OutBorder4.GoogleDriveService(
                configService.SettingsDict);
            RegisterByFunc(() => driveService);

            var docsService = OutBorder5.GoogleDocsService(
                configService.SettingsDict);
            RegisterByFunc(() => docsService);

            var sheetService = OutBorder6.GoogleSheetService(
                configService.SettingsDict);
            RegisterByFunc(() => sheetService);

            var videoService = OutBorder8.VideoService(fileService);

            var ttsService = OutBorder7.TtsService(fileService, repoService, videoService);
            RegisterByFunc(() => ttsService);
        }
    }
}