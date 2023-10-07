using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using Unity;

namespace SharpConfigProg.Repetition
{
    public static partial class OutBorder
    {
        public static IConfigService ConfigService(
            IFileService fileService)
        {
            if (!MyBorder.Container.IsRegistered<IConfigService>())
            {
                MyBorder.Registration
                    .RegisterByFunc<IFileService>(() => fileService);

                new PreparerRegistration().Invoke();

                MyBorder.Registration.RegisterByFunc<IConfigService>(
                        () => new ConfigService(fileService));
            }

            var configService = MyBorder.Container.Resolve<IConfigService>();
            return configService;
        }
    }
}
