using SharpFileServiceProg.Operations.Files;
using SharpFileServiceProg.Operations.Headers;
using SharpFileServiceProg.Operations.Index;
using SharpFileServiceProg.Operations.RepoAddress;
using static SharpFileServiceProg.Service.IFileService;

namespace SharpFileServiceProg.Service
{
    internal partial class FileService : IFileService
    {
        public IFileWrk File { get; private set; }
        public IIndexWrk Index { get; private set; }
        public IYamlWrk Yaml { get; private set; }
        public IPathsOperations Path { get; private set; }
        public HeadersOperations Header { get; private set; }
        public IRepoAddressOperations RepoAddress { get; private set; }

        public FileService()
        {
            File = new FileWrk(this);
            Index = new IndexOperations();
            Yaml = new YamlWorker();
            Path = new PathsOperations();
            Header = new HeadersOperations();
            RepoAddress = new RepoAddressOperations(Index);
        }
    }

    internal partial class FileService
    {
        
    }
}
