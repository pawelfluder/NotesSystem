using SharpFileServiceProg.Service;
using SharpVideoServiceProg.Service;

namespace SharpVideoServiceProg.AAPublic
{
    public static class OutBorder
    {
        public static IVideoService VideoService(
            IFileService fileService)
        {
            return new VideoService(fileService);
        }
    }
}
