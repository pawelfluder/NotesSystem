using SharpFileServiceProg.Operations.FileSize;
using SharpFileServiceProg.Operations.Yaml;
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

        internal class YamlWorker : IFileService.IYamlWrk
        {
            public IFileService.IYamlOperations Dotnet { get; }
            public IFileService.IYamlOperations Sharp { get; }
            public IFileService.IYamlOperations Byjson { get; }
            public IFileService.IYamlOperations Custom01 { get; }
            public IFileService.IYamlOperations Custom02 { get; }
            public IFileService.IYamlOperations Custom03 { get; }

            public YamlWorker()
            {
                Dotnet = new DotnetYamlOperations();
                Sharp = new SharpYamlOperations();
                Byjson = new ByJsonYamlOperations();
                Custom01 = new Custom01YamlOperations();
                Custom02 = new Custom02YamlOperations();
                Custom03 = new Custom03YamlOperations();
            }
        }
    }
}
