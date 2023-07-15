using SharpFileServiceProg.Operations.Files;
using SharpFileServiceProg.Operations.Headers;
using static SharpFileServiceProg.Service.FileService;

namespace SharpFileServiceProg.Service
{
    public partial interface IFileService
    {
        IFileWrk File { get; }
        IIndexWrk Index { get; }
        IYamlWrk Yaml { get; }
        IPathsOperations Path { get; }
        HeadersOperations Header { get; }
    }
}