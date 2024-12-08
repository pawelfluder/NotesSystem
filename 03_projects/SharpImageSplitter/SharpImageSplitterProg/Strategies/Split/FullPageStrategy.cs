using SharpImageSplitterProg.AAPublic.Strategies;

namespace SharpImageSplitterProg.Strategies.Split;

public class FullPageStrategy : ISplitStrategy
{
    public decimal HeightByWidth => 1.46m;
    public decimal Overlap => 1;
}
