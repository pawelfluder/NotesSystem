using SharpConfigProg.Service;
using SharpFileServiceProg.Service;

namespace SharpConfigProg.Repetition
{
    public static class OutBorder
    {
        public static IConfigService ConfigService(
            IFileService fileService)
        {
            return new ConfigService(fileService);
        }
    }
}
