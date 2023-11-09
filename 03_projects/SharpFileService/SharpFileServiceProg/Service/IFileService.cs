using SharpConfigProg.AAPublic;
using SharpFileServiceProg.AAPublic;
using SharpFileServiceProg.Operations.Files;
using SharpFileServiceProg.Operations.Headers;
using SharpFileServiceProg.Operations.RepoAddress;

namespace SharpFileServiceProg.Service
{
    public partial interface IFileService
    {
        IFileWrk File { get; }
        IIndexWrk Index { get; }
        IYamlWrk Yaml { get; }
        IPathsOperations Path { get; }
        HeadersOperations Header { get; }
        IRepoAddressOperations RepoAddress { get; }
        IGoogleCredentialWorker Credentials { get; }
        IReflectionOperations Reflection { get; }
    }
}