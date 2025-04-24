using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.Duplications.Operations.Files;
using SharpRepoServiceProg.Registrations;

namespace SharpRepoServiceProg.Operations.Files;

internal class FileOperations
{
    public IRepoAddressesObtainer NewRepoAddressesObtainer()
    {
        IFileService fileService = MyBorder.OutContainer.Resolve<IFileService>();
        CustomOperationsService customOperationsService = MyBorder.MyContainer.Resolve<CustomOperationsService>();
        GetRepoAddresses obtainer =  new (fileService, customOperationsService);
        return obtainer;
    }
}