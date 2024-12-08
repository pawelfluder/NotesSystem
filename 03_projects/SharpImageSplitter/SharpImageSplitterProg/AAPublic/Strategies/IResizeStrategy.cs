namespace SharpImageSplitterProg.AAPublic.Strategies;

public interface IResizeStrategy
{
    string ValidOption { get; }
    public (int Width, int Height) ResizeWidthQHeight { get; }
    public int DesireWidth { get; }
    public int DesireHeight { get; }
}