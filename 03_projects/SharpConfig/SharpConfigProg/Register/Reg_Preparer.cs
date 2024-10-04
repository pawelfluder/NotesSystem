using SharpConfigProg.AAPublic;
using SharpConfigProg.Service;

namespace SharpConfigProg.Register
{
    internal class Reg_Preparer
    {
        public void Register(IOperationsService operationsService)
        {
            MyBorder.Registration
                .RegisterByFunc<IPreparer>(()
                => new GuidFolderPreparer(fileService));
        }
    }
}
