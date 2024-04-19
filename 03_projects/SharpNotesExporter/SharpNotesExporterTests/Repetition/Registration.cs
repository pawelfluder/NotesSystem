using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using Unity;
using OutBorder1 = SharpFileServiceProg.Repetition.OutBorder;
using OutBorder2 = SharpConfigProg.Repetition.OutBorder;

namespace SharpNotesExporterTests.Repetition
{
    internal class Registration : RegistrationBase
    {
        protected override void Registrations()
        {
            RegisterByFunc<IFileService>(OutBorder1.FileService);
            RegisterByFunc<IConfigService, IFileService>(
                OutBorder2.ConfigService,
                container.Resolve<IFileService>());
        }
    }
}
