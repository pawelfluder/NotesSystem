using SharpContainerProg.AAPublic;

namespace SharpRepoBackendProg.Repetition
{
    internal static class MyBorder
    {
        public static Registration Registration = new Registration();
        public static IContainer Container => Registration.Start();

        //public static GoogleDocsService GoogleDocsService()
        //{
        //    var aplicationName = "";
        //    var scopes = new List<string>();
        //    GoogleDocsService googleDocsService;
        //    if (GetCredenctials(out var clientId, out var clientSecret))
        //    {
        //        googleDocsService = new GoogleDocsService(
        //            clientId.ToString(),
        //            clientSecret.ToString(),
        //            aplicationName,
        //            scopes);
        //        return googleDocsService;
        //    }

        //    googleDocsService = new GoogleDocsService();
        //    return googleDocsService;
        //}

        //public static GoogleDriveService NewGoogleDriveService()
        //{
        //    GoogleDriveService googleDocsService;
        //    if (GetCredenctials(out var clientId, out var clientSecret))
        //    {
        //        googleDocsService = new GoogleDriveService(
        //            clientId.ToString(),
        //            clientSecret.ToString());
        //        return googleDocsService;
        //    }

        //    googleDocsService = new GoogleDriveService();
        //    return googleDocsService;
        //}

        //private static bool GetCredenctials(out string clientId, out string clientSecret)
        //{
        //    var configService = Container.Resolve<IConfigService>();
        //    var s1 = configService.TryGetSettingAsString("googleClientId", out clientId);
        //    var s2 = configService.TryGetSettingAsString("googleClientSecret", out clientSecret);
        //    return s1 && s2;
        //}
    }
}
