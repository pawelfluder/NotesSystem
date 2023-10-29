using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using Border1 = SharpFileServiceProg.AAPublic.OutBorder;
using Border2 = SharpConfigProg.AAPublic.OutBorder;
using Unity;

namespace SharpRepoBackendProg.Repetition
{
    internal class Registration : RegistrationBase
    {
        protected override void Registrations()
        {
            RegisterByFunc<IFileService>(Border1.FileService);
            RegisterByFunc<IConfigService, IFileService>(
                Border2.ConfigService,
                container.Resolve<IFileService>());
        }
    }
}
