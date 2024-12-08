using SharpImageSplitterProg.AAPublic;

namespace SharpImageSplitterProg.Service;

public interface IImageSpliterService
{
    ISplitterJob Splitter { get; }
}