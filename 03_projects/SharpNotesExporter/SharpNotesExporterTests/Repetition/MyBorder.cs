using GoogleDocsServiceProj.Service;
using SharpConfigProg.Service;
using SharpGoogleDriveProg.Service;
using Unity;
using SharpConfigProg.Preparer;
namespace SharpNotesExporterTests.Repetition
{
    internal static class MyBorder
    {
        private static UnityContainer container = new Registration().Start();
        public static UnityContainer Container => container;

        public static GoogleDocsService GoogleDocsService()
        {
            //var fileService = Container.Resolve<IFileService>();
            var configService = container.Resolve<IConfigService>();

            var clientId = configService.SettingsDict["googleClientId"].ToString();
            var clientSecret = configService.SettingsDict["googleClientSecret"].ToString();

            var aplicationName = "";
            var scopes = new List<string>();
            var googleDocsService = new GoogleDocsService(
                clientId,
                clientSecret,
                aplicationName,
                scopes);
            return googleDocsService;
        }

        public static GoogleDriveService NewGoogleDriveService()
        {
            var configService = container.Resolve<IConfigService>();
            configService.Prepare(typeof(IPreparer.INotesSystem));

            var clientId = configService.SettingsDict["googleClientId"].ToString();
            var clientSecret = configService.SettingsDict["googleClientSecret"].ToString();

            var googleDocsService = new GoogleDriveService(
                clientId,
                clientSecret);
            return googleDocsService;
        }
    }
}
