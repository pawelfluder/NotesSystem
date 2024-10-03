using SharpFileServiceProg.Operations.FileSize;
using SharpRepoServiceProg.FileOperations;

namespace SharpFileServiceProg.Service
{
    internal partial class FileService
    {
        internal class FileWrk : IFileService.IFileWrk
        {
            private readonly IFileService fileService;

            public FileWrk(IFileService fileService)
            {
                this.fileService = fileService;
            }

            public IFileService.IVisit GetNewRecursivelyVisitDirectory()
                => new VisitDirectoriesRecursively();

            public IFileService.IParentVisit GetNewVisitDirectoriesRecursivelyWithParentMemory()
                => new VisitDirectoriesRecursivelyWithParentMemory();

            public IRepoAddressesObtainer NewRepoAddressesObtainer()
                => new GetRepoAddresses(fileService);
        }
    }
}
