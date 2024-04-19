using SharpConfigProg.Service;
using SharpContainerProg.AAPublic;
using SharpGoogleDriveProg.AAPublic;
using OutBorder04 = SharpGoogleDriveProg.AAPublic.OutBorder;

namespace SharpGoogleDriveTests.Registrations
{
    internal static class MyBorder
    {
        private static IContainer container = new Registration().Start();
        public static IContainer Container => container;

        public static IGoogleDriveService GoogleSheetService()
        {
            var configService = Container.Resolve<IConfigService>();
            var driveService = Container.Resolve<IGoogleDriveService>();
            var service = OutBorder04.GoogleDriveService(configService.SettingsDict);
            return service;
        }
    }
}