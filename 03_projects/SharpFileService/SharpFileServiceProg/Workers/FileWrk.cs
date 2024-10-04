using SharpFileServiceProg.AAPublic;
using SharpFileServiceProg.Recursively;

namespace SharpFileServiceProg.Workers
{
    internal class FileWrk : IFileWrk
    {
        private readonly IFileService fileService;

        public FileWrk(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public IFileVisit GetNewRecursivelyVisitDirectory()
            => new VisitDirectoriesRecursively();

        public IParentVisit GetNewVisitDirectoriesRecursivelyWithParentMemory()
            => new VisitDirectoriesRecursivelyWithParentMemory();
    }
}