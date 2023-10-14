using SharpConfigProg.ConfigPreparer;
using SharpConfigProg.Service;
using SharpContainerProg.Public;
using SharpFileServiceProg.Service;

namespace SharpConfigProg.Repetition
{
    internal class Registration : RegistrationBase
    {
        protected override void Registrations()
        {
            RegisterByFunc<IPreparer, IFileService>(
                (x) => new GuidFolderPreparer(x),
                () => container.Resolve<IFileService>());
        }
    }
}
