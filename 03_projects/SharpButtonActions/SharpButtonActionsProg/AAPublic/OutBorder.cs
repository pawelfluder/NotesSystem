using SharpButtonActionsProg.Service;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpButtonActionsProg.AAPublic;

public static class OutBorder
{
    public static ISystemActionsService SytemActionsService(
        IOperationsService operationsService)
    {
        return new SystemActionsService(operationsService);
    }
}