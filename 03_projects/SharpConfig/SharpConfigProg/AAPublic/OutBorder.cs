using SharpConfigProg.Register;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;

namespace SharpConfigProg.AAPublic
{
    public static partial class OutBorder
    {
        public static IConfigService ConfigService(
            IFileService fileService)
        {
            if (!MyBorder.Container.IsRegistered<IPreparer>())
            {
                new Reg_Preparer().Register(fileService);
            }

            var configService = new ConfigService(fileService);
            return configService;
        }
    }
}
