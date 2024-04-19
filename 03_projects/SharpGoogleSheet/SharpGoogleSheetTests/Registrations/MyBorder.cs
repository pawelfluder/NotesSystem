using SharpConfigProg.Service;
using SharpContainerProg.AAPublic;
using SharpGoogleSheetProg.AAPublic;
using OutBorder04 = SharpGoogleSheetProg.AAPublic.OutBorder;

namespace SharpGoogleSheetTests.Registrations
{
    internal static class MyBorder
    {
        private static IContainer container = new Registration().Start();
        public static IContainer Container => container;

        public static IGoogleSheetService GoogleSheetService()
        {
            var configService = MyBorder.Container.Resolve<IConfigService>();
            var service = OutBorder04.GoogleSheetService(configService.SettingsDict);
            return service;
        }
    }
}