namespace SharpOperationsProg.AAPublic.Operations;

public interface IFileWrk
{
    IVisit GetNewRecursivelyVisitDirectory();
    IParentVisit GetNewVisitDirectoriesRecursivelyWithParentMemory();
    IRepoAddressesObtainer NewRepoAddressesObtainer();
}