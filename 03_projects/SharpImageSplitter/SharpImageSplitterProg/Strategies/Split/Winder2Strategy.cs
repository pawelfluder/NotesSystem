using SharpImageSplitterProg.AAPublic.Strategies;

namespace SharpImageSplitterProg.Strategies.Split;

public class Winder2Strategy : ISplitStrategy
{
    public decimal HeightByWidth => 2.0m;
    public decimal Overlap => 5;
}
// iPhone 11 Pro Max iOS 14.6
// 414 x 896 -> 450 x 900