using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using Unity;

namespace SharpConfigProg.Repetition
{
    internal partial class PreparerRegistration : AdditionalRegistrationBase
    {
        public void Register()
        {
            var fileService = MyBorder.Container.Resolve<IFileService>();

            MyBorder.Registration
                .RegisterByFunc<IConfigService.ILocalProgramDataPreparer>(()
                => new LocalProgramDataPreparer(fileService));
        }
    }
}
