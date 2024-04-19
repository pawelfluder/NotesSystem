using SharpButtonActionsProj.Service;
using SharpFileServiceProg.Service;
namespace SharpButtonActionsProg.AAPublic
{
    public static class OutBorder
    {
        public static ISystemActionsService SytemActionsService(IFileService fileService)
        {
            return new SystemActionsService(fileService);
        }
    }
}
