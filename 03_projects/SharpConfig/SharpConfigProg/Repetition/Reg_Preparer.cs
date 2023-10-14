using SharpConfigProg.ConfigPreparer;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using Unity;

namespace SharpConfigProg.Repetition
{
    internal partial class Reg_Preparer : AdditionalRegistrationBase
    {
        public void Register()
        {
            var fileService = MyBorder.Container.Resolve<IFileService>();

            MyBorder.Registration
                .RegisterByFunc<IPreparer>(()
                => new GuidFolderPreparer(fileService));
        }
    }
}
