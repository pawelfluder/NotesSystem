using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.Registration;

namespace SharpRepoServiceProg.Operations.Files;

internal class FileOperations
{
    public IRepoAddressesObtainer NewRepoAddressesObtainer()
    {
        IFileService fileService = MyBorder.Container.Resolve<IFileService>();
        OperationsService operationsService = MyBorder.Container.Resolve<OperationsService>();
        GetRepoAddresses obtainer =  new (fileService, operationsService);
        return obtainer;
    }
}