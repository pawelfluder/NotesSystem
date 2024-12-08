using System.Data;
using SharpImageSplitterProg.AAPublic.Strategies;

namespace SharpImageSplitterProg.Strategies.Resize;

internal class HalfPageStrategy : IResizeStrategy
{
    public string ValidOption => nameof(DesireWidth);
    public (int Width, int Height) ResizeWidthQHeight => throw new InvalidExpressionException();
    public int DesireWidth => 260;
    public int DesireHeight => throw new InvalidExpressionException();
}