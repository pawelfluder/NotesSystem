using SharpFileServiceProg.AAPublic;

namespace SharpOperationsProg.AAPublic
{
    public class OutBorder
    {
        public static IOperationsService operationsService()
        {
            return new FileService();
        }
    }
}
