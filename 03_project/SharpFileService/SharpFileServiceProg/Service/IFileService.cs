using SharpFileServiceProg.Operations.Files;
using static SharpFileServiceProg.Service.FileService;

namespace SharpFileServiceProg.Service
{
    public partial interface IFileService
    {
        IFileWrk File { get; }
        IIndexWrk Index { get; }
        IYamlWrk Yaml { get; }
        IPathsOperations Path { get; }
    }
}