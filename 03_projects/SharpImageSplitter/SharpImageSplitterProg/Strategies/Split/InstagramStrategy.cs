using SharpImageSplitterProg.AAPublic.Strategies;

namespace SharpImageSplitterProg.Strategies.Split;
public class InstagramStrategy : ISplitStrategy
{
    public decimal HeightByWidth => 2.0m;
    public decimal Overlap => 5;
}
// instagram
// 450 = 109 + 341
// 559 = 109 + 450
// 559 x 900