using SharpConfigProg.Register;
using SharpConfigProg.Service;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpConfigProg.AAPublic;

public static partial class OutBorder
{
    public static IConfigService ConfigService(
        IOperationsService operationsService)
    {
        if (!MyBorder.Container.IsRegistered<IPreparer>())
        {
            new Reg_Preparer().Register(operationsService);
        }
            
        var configService = new ConfigService(operationsService);
        return configService;
    }
}