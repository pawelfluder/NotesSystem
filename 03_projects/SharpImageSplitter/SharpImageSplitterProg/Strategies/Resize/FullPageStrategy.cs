using System.Data;
using SharpImageSplitterProg.AAPublic.Strategies;

namespace SharpImageSplitterProg.Strategies.Resize;

internal class FullPageStrategy : IResizeStrategy
{
    public string ValidOption => nameof(DesireWidth);
    public (int Width, int Height) ResizeWidthQHeight => throw new InvalidExpressionException();
    public int DesireWidth => 525;
    public int DesireHeight => throw new InvalidExpressionException();
}