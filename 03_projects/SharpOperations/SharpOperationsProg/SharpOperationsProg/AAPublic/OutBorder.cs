using SharpFileServiceProg.AAPublic;

namespace SharpOperationsProg.AAPublic
{
    public class OutBorder
    {
        public static IFileService FileService()
        {
            return new FileService();
        }
    }
}
