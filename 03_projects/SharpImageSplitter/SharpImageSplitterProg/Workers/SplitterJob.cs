using System;
using SharpImageSplitterProg.AAPublic;
using SharpImageSplitterProg.AAPublic.Strategies;
using SharpImageSplitterProg.Models;
using SharpOperationsProg.AAPublic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SharpImageSplitterProg.Workers;

internal class SplitterJob : ISplitterJob
{
    internal readonly StrategyBase<ISplitStrategy> _strategyBase;
    internal readonly PathsJob _paths;
    internal readonly RecalculateJob _recalculate;

    public SplitterJob()
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
        string tempFolderPath = _paths.GetTempFolderFilePath(folderQfile);
        string inputFilePath = _paths.GetInputImageFilePath(folderQfile);
        
        Image image = Image.Load(inputFilePath);
        SplitInfo splitInfo = CreateSplitImages(
            image,
            heightByWidthRatio,
            overlapPercentage,
            tempFolderPath,
            topOffset);
        
        _paths.AddPaths(
            folderQfile,
            splitInfo);
        
        return splitInfo;
    }

    internal SplitInfo CreateSplitImages(
        Image image,
        decimal heightByWidthRatio,
        decimal overlapPercentage,
        string tempFolderPath,
        int topOffset)
    {
        SplitInfo splitInfo = Do(
            image,
            heightByWidthRatio,
            overlapPercentage,
            tempFolderPath,
            topOffset);
        
        _paths.AddTempFolderPath(
            tempFolderPath,
            splitInfo);
        
        return splitInfo;
    }

    internal SplitInfo Do(
        Image image,
        decimal heightByWidthRatio,
        decimal overlapPercentage,
        string tempFolderPath,
        int topOffset)
    {
        _paths.ReCreateNewTempFolder(tempFolderPath);
        SplitInfo splitInfo = _recalculate.Do(
            heightByWidthRatio,
            overlapPercentage,
            image.Width,
            image.Height,
            topOffset);
        
        for (int i = 0; i < splitInfo.ImagesCount; i++)
        {
            Rectangle cropRectangle = splitInfo
                .RectanglesArray[i];
            
            try
            {
                string outputfilePath = _paths
                    .GetOutputFilePath(i, tempFolderPath);
                Image clonedImage = image.Clone(ctx =>
                    ctx.Crop(cropRectangle));
                clonedImage.Save(outputfilePath);
            }
            catch(Exception ex)
            {
                var tmp = splitInfo.EverythingToString();
                throw ex;
            }
        }
            
        image.Dispose();
        return splitInfo;
    }
}
