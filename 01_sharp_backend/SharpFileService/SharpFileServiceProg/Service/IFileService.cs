using static SharpFileServiceProg.Service.FileService;

namespace SharpFileServiceProg.Service
{
    public interface IFileService
    {
        public IFileWrk File { get; }
        public IIndexWrk Index { get; }
    }
}