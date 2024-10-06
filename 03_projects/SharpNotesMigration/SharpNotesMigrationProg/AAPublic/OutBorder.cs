using SharpNotesMigrationProg.Service;
using SharpOperationsProg.AAPublic.Operations;
using SharpRepoServiceProg.AAPublic;

namespace SharpNotesMigrationProg.AAPublic;

public class OutBorder
{
    public static IMigrationService MigrationService(
        IOperationsService operationsService,
        IRepoService repoService)
    {
        var service = new MigrationService(operationsService, repoService);
        return service;
    }
}