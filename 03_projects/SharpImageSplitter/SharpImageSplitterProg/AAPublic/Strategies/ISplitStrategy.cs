namespace SharpImageSplitterProg.AAPublic.Strategies;

public interface ISplitStrategy
{
    decimal HeightByWidth { get; }
    decimal Overlap { get; }
}