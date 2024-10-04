using SharpFileServiceProg.AAPublic;
using SharpFileServiceProg.Recursively;

namespace SharpFileServiceProg.Workers
{
    internal class FileWrk : IFileWrk
    {
        private readonly IFileService operationsService;

        public FileWrk(IFileService operationsService)
        {
            this.operationsService = operationsService;
        }

        public IFileVisit GetNewRecursivelyVisitDirectory()
            => new VisitDirectoriesRecursively();

        public IParentVisit GetNewVisitDirectoriesRecursivelyWithParentMemory()
            => new VisitDirectoriesRecursivelyWithParentMemory();
    }
}