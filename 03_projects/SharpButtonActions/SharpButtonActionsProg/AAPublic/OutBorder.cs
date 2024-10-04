using SharpButtonActionsProj.Service;
using SharpOperationsProg.AAPublic;

namespace SharpButtonActionsProg.AAPublic
{
    public static class OutBorder
    {
        public static ISystemActionsService SytemActionsService(IOperationsService operationsService)
        {
            return new SystemActionsService(operationsService);
        }
    }
}
