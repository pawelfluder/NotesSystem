using SixLabors.ImageSharp;

namespace SharpImageSplitterProg.Workers;

using SharpImageSplitterProg.Models;
internal class RecalculateJob
{
    private readonly HeightInfoEngineJob _engine  = new();
    public SplitInfo Do(
        decimal heightByWidthRatio,
        decimal overlapPercentage,
        int imageWidth,
        int imageHeight,
        int topOffset = 0)
    {
        SplitInfo info = new();
        info.HWRatio = heightByWidthRatio;
        info.Olap = overlapPercentage;
        info.Wmax = imageWidth;
        info.Hmax = imageHeight;
        info.TopOffset = topOffset;

        info.HeightCrop = _engine.GetHeightCrop(info); // 1)
        info.HeightMiddle = _engine.GetHeightMiddle(info); // 2)
        info.Strap = _engine.GetStrap(info); // 3)
        info.FirstHeightMiddle = _engine.GetFirstHeightMiddle(info); // 4)
        info.FirstStrapBottom = _engine.GetFirstStrapBottom(info); // 5)
        info.ImagesCovering = _engine.GetImagesCovering(info); // 6)
        info.ImagesCount = _engine.GetImagesCount(info); // 7)
        // 8) _lastSumStrapPlusHeightMiddle
        // ( is set in Images property setter )
        // ( do not use in public calculations - _lastSumStrapPlusHeightMiddle )
        info.LastStrapBottom = _engine.GetLastStrapBottom(info); // 9)
        info.LastHeightMiddle = _engine.GetLastHeightMiddle(info); // 10)
        
        for (int i = 0; i < info.ImagesCount; i++)
        {
            AddNewToHeightInfoArray(i, info);
            AddNewToRectanglesArray(i, info);
        }

        return info;
    }
    
    private void AddNewToHeightInfoArray(
        int i,
        SplitInfo info)
    {
        HeightInfo heightInfo = _engine.GetHeightInfo(i, info);
        info.HeightInfosArray[i] = heightInfo;
    }

    public Rectangle AddNewToRectanglesArray(
        int i,
        SplitInfo info)
    {
        HeightInfo hInfo = info.HeightInfosArray[i];
        int xPosition = 0;
        int yPosition = hInfo.YSHC_StartPosOfHeightCrop;
        int width = info.Wmax;
        int height = hInfo.HeightCrop;
        Rectangle rectangle = new(
            xPosition,
            yPosition,
            width,
            height);
        info.RectanglesArray[i] = rectangle;
        return rectangle;
    }
}
