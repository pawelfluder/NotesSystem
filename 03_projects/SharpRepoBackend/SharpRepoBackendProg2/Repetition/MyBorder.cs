using GoogleDocsServiceProj.Service;
using SharpConfigProg.Service;
using SharpGoogleDriveProg.Service;
using Unity;

namespace SharpRepoBackendProg.Repetition
{
    internal static class MyBorder
    {
        public static Registration Registration = new Registration();
        public static UnityContainer Container => Registration.TryInitialize();

        public static GoogleDocsService GoogleDocsService()
        {
            var configService = Container.Resolve<IConfigService>();

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
            var configService = Container.Resolve<IConfigService>();

            var clientId = configService.SettingsDict["googleClientId"].ToString();
            var clientSecret = configService.SettingsDict["googleClientSecret"].ToString();

            var googleDocsService = new GoogleDriveService(
                clientId,
                clientSecret);
            return googleDocsService;
        }
    }
}
