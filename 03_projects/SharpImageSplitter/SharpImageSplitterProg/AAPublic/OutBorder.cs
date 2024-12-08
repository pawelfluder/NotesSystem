using SharpImageSplitterProg.Service;

namespace SharpImageSplitterProg.AAPublic;

public static class OutBorder
{
    public static IImageSpliterService ImageSpliterService()
    {
        ImageSpliterService service = new();
        return service;
    }
}

