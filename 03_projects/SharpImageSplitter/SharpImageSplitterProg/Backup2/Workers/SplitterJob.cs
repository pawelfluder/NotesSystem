using SharpImageSplitterProg.AAPublic;
using SharpImageSplitterProg.AAPublic.Strategies;
using SharpImageSplitterProg.Models;
using SharpImageSplitterProg.Workers;
using SharpOperationsProg.AAPublic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SharpImageSplitterProg.Backup2.Workers;

internal class SplitterJob3 : ISplitterJob
{
    internal readonly StrategyBase<ISplitStrategy> _strategyBase;
    internal readonly PathsJob _paths;
    internal readonly RecalculateJob _recalculate;
    internal SplitInfo _splitInfo;

    public SplitterJob3()
    {
        _paths = new PathsJob();
        _strategyBase = new StrategyBase<ISplitStrategy>();
        _recalculate = new RecalculateJob();
    }
    
    public void CreateSplitImages(
        (string folderPath, string fileName) folderQfile,
        string splitStrategyName)
    {
        ISplitStrategy strategy = _strategyBase.GetNewStrategy(splitStrategyName);
        CreateSplitImages(folderQfile, strategy.HeightByWidth, strategy.Overlap);
    }
    internal SplitInfo CreateSplitImages(
        (string folderPath, string fileName) folderQfile,
        decimal heightByWidthRatio,
        decimal overlapPercentage,
        int topOffset = 0)
    {
        string tempFolderPath = _paths
            .GetTempFolderFilePath(folderQfile);
        string inputFilePath = _paths
            .GetInputImageFilePath(folderQfile);
        
        Image image = Image.Load(inputFilePath);
        CreateSplitImages(
            image,
            heightByWidthRatio,
            overlapPercentage,
            tempFolderPath);
        return _splitInfo;
    }

    internal SplitInfo CreateSplitImages(
        Image image,
        decimal heightByWidthRatio,
        decimal overlapPercentage,
        string tempFolderPath,
        int topOffset = 0)
    {
        _splitInfo = _recalculate.Do(
            heightByWidthRatio,
            overlapPercentage,
            image.Width,
            image.Height,
            topOffset);
        
        for (int i = 0; i < _splitInfo.ImagesCount; i++)
        {
            Rectangle cropRectangle = _recalculate
                .AddNewToRectanglesArray(i, _splitInfo);
            // Rectangle cropRectangle2 =_recalculate
            //     .RectangleByPositionIntDict(i, _splitInfo);
            
            try
            {
                string outputfilePath = _paths
                    .GetOutputFilePath(i, tempFolderPath);
                Image clonedImage = image.Clone(ctx =>
                    ctx.Crop(cropRectangle));
                clonedImage.Save(outputfilePath);
            }
            catch
            {
                // ignored
            }
        }
            
        image.Dispose();
        return _splitInfo;
    }
}