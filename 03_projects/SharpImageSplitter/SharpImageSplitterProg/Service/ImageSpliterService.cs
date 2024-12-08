using SharpImageSplitterProg.AAPublic;
using SharpImageSplitterProg.Workers;

namespace SharpImageSplitterProg.Service;

public class ImageSpliterService : IImageSpliterService
{
    private ISplitterJob? _splitter;
    private bool isSplitterInit;

    public ISplitterJob Splitter
    {
        get
        {
            if (!isSplitterInit)
            {
                _splitter = new SplitterJob();
                isSplitterInit = true;
            }
                
            return _splitter;
        }
    }
}
