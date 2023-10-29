using SharpConfigProg.AAPublic;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;

namespace SharpConfigProg.Register
{
    internal class Reg_Preparer
    {
        public void Register(IFileService fileService)
        {
            MyBorder.Registration
                .RegisterByFunc<IPreparer>(()
                => new GuidFolderPreparer(fileService));
        }
    }
}
