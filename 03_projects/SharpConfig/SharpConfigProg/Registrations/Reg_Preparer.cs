using SharpConfigProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpConfigProg.Registrations;

internal class Reg_Preparer
{
    public void Register(IOperationsService operationsService)
    {
        MyBorder.OutContainer
            .RegisterByFunc<IPreparer>(()
                => new GuidFolderPreparer(operationsService));
    }
}
