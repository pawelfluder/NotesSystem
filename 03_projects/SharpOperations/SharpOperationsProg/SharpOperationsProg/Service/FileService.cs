using SharpFileServiceProg;
using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SharpOperationsProg.Operations.Credentials;
using SharpOperationsProg.Operations.Headers;
using SharpOperationsProg.Operations.Index;
using SharpOperationsProg.Operations.Json;
using SharpOperationsProg.Operations.Path;
using SharpOperationsProg.Operations.Reflection;
using SharpOperationsProg.Operations.RepoAddress;

namespace SharpOperationsProg.Service
{
    internal class OperationService : IFileService
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

        public OperationService()
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