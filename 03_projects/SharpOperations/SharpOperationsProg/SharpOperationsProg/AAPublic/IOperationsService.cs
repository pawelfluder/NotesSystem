using SharpFileServiceProg;
using SharpOperationsProg.AAPublic.Operations;
using SharpOperationsProg.Operations.Headers;
using SharpOperationsProg.Operations.Path;
using SharpOperationsProg.Operations.RepoAddress;
using SharpOperationsProg.Operations.UniItem;

namespace SharpOperationsProg.AAPublic;

public interface IOperationsService
{
    IFileWrk File { get; }
    IIndexWrk Index { get; }
    IYamlWrk Yaml { get; }
    IPathsOperations Path { get; }
    HeadersOperations Header { get; }
    IRepoAddressOperations RepoAddress { get; }
    IUnitItemOperations RepoItem { get; }
    IGoogleCredentialWorker Credentials { get; }
    IReflectionOperations Reflection { get; }
    IJsonOperations Json { get; }
}