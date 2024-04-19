using SharpConfigProg.AAPublic;
using SharpConfigProg.Credentials;
using SharpFileServiceProg.AAPublic;
using SharpFileServiceProg.Operations.Files;
using SharpFileServiceProg.Operations.Headers;
using SharpFileServiceProg.Operations.Index;
using SharpFileServiceProg.Operations.Json;
using SharpFileServiceProg.Operations.Reflection;
using SharpFileServiceProg.Operations.RepoAddress;
using SharpFileServiceProg.ServiceWorkers;
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
        public IGoogleCredentialWorker Credentials { get; private set; }
        public IReflectionOperations Reflection { get; private set; }
        public IJsonOperations Json { get; private set; }

        public FileService()
        {
            File = new FileWrk(this);
            Index = new IndexOperations();
            Yaml = new YamlWorker();
            Path = new PathsOperations();
            Header = new HeadersOperations();
            RepoAddress = new RepoAddressOperations(Index);
            Credentials = new GoogleCredentialWorker();
            Reflection = new ReflectionOperations();
            Json = new JsonOperations();
        }
    }
}
