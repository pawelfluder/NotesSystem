using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.Operations.Files;

namespace SharpOperationsProg.AAPublic.Operations;

public interface IFileWrk
{
    IFileVisit GetNewRecursivelyVisitDirectory();
    IParentVisit GetNewVisitDirectoriesRecursivelyWithParentMemory();
    IRepoAddressesObtainer NewRepoAddressesObtainer();
}