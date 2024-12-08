using SharpImageSplitterProg.AAPublic.Strategies;

namespace SharpImageSplitterProg.Strategies.Split;

public class Winder1Strategy : ISplitStrategy
{
    public decimal HeightByWidth => 2.5m;
    public decimal Overlap => 5;
}