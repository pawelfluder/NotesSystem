using SharpConfigProg.ConfigPreparer;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using Unity;

namespace SharpConfigProg.Repetition
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
