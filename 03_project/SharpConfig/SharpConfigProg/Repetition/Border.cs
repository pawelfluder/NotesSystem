using SharpConfigProg.Service;
using SharpFileServiceProg.Service;

namespace SharpConfigProg.Repetition
{
    public static class Border
    {
        public static IConfigService NewConfigService(
            IFileService fileService)
        {
            return new ConfigService(fileService);
        }
    }
}
