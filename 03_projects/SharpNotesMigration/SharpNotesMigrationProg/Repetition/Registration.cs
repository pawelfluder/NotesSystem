using SharpConfigProg.Service;
using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using Border1 = SharpFileServiceProg.AAPublic.OutBorder;
using Border2 = SharpOperationsProg.AAPublic.OutBorder;
using Border3 = SharpConfigProg.AAPublic.OutBorder;

namespace SharpNotesMigrationProg.Repetition
{
    internal class Registration : RegistrationBase
    {
        protected override void Registrations()
        {
            var fileService = Border1.FileService();
            RegisterByFunc<IFileService>(() => fileService);
            
            var operationsService = Border2.OperationsService(fileService);
            RegisterByFunc<IOperationsService>(() => operationsService);

            var configService = Border3.ConfigService(operationsService);
            RegisterByFunc<IConfigService>(() => configService);
        }
    }
}