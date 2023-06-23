using GoogleDocsServiceProj.Service;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpGoogleDriveProg.Service;
using Border1 = SharpFileServiceProg.Repetition.Border;
using Border2 = SharpConfigProg.Repetition.Border;
using Unity;
using Unity.Injection;
using SharpConfigProg.Preparer;

namespace SharpRepoBackendProg.Repetition
{
    internal static class MyBorder
    {
        private static IFileService fileService;
        private static IConfigService configService;
        public static UnityContainer container = Start();
        public static UnityContainer Container => container;

        private static UnityContainer Start()
        {
            container = new UnityContainer();
            Registrations();
            return container;
        }

        private static void Registrations()
        {
            RegisterByFunc<IFileService>(Border1.NewFileService);
            RegisterByFunc<IConfigService, IFileService>(
                Border2.NewConfigService,
                container.Resolve<IFileService>());
        }

        public static void RegisterByFunc<R>(Func<R> func)
        {
            var factory = new InjectionFactory(c =>
            {
                return func.Invoke();
            });
            container.RegisterSingleton<R>(factory);
        }

        public static void RegisterByFunc<R, T1>(Func<T1, R> func, T1 t1)
        {
            container.RegisterSingleton<R>(new InjectionFactory(c =>
            {
                return func.Invoke(t1);
            }));
        }

        public static void RegisterSingleton<R, T1, T2>(Func<T1, T2, R> func, T1 t1, T2 t2)
        {
            container.RegisterType<R>(new InjectionFactory(c =>
            {
                return func.Invoke(t1, t2);
            }));
        }

        public static GoogleDocsService GoogleDocsService()
        {
            //fileService = Border1.NewFileService();
            //configService = Border2.NewConfigService(fileService);
            //configService.Prepare(typeof(IPreparer.INotesSystem));

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
