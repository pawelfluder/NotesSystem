using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.Operations.Headers;
using SharpOperationsProg.Operations.Path;
using SharpOperationsProg.Operations.UniAddress;
using SharpOperationsProg.Operations.UniItem;

namespace SharpOperationsProg.AAPublic.Operations;

public interface IOperationsService
{
    IFileWrk File { get; }
    IIndexWrk Index { get; }
    // IYamlWrk Yaml { get; }
    IPathsOperations Path { get; }
    HeadersOperations Header { get; }
    IUniAddressOperations UniAddress { get; }
    IUnitItemOperations UniItem { get; }
    IGoogleCredentialWorker Credentials { get; }
    IReflectionOperations Reflection { get; }
    IJsonOperations Json { get; }
    IFileService GetFileService();
}