using System;
using SharpImageSplitterProg.Models;

namespace SharpImageSplitterProg.Workers;

internal class HeightInfoEngineJob
{
    // 1)
    public int GetHeightCrop(SplitInfo info)
    {
        int result = (int)(info.HWRatio * info.Wmax);
        return result;
    }
    
    // 2)
    public int GetHeightMiddle(SplitInfo info)
    {
        decimal result = (1 - (decimal)2/100 * info.Olap) * info.HeightCrop;
        return (int)result;
    }
    
    // 3)
    public int GetStrap(SplitInfo info)
    {
        decimal result = ((decimal)1/100 * info.Olap) * info.HeightCrop;
        return (int)result;
    }
    
    // 4)
    public int GetFirstHeightMiddle(
        SplitInfo info)
    {
        if (info.TopOffset >= info.HeightCrop - info.Strap)
        {
            throw new Exception();
        }
        
        int result =
            info.HeightCrop
            - info.Strap
            - info.TopOffset;
        return result;
    }
    
    // 5)
    public int GetFirstStrapBottom(SplitInfo info)
    {
        if (info.FirstHeightMiddle <= (info.HeightCrop - info.Strap))
        {
            return info.Strap;
        }
        int result = info.HeightCrop - info.FirstHeightMiddle;
        return result;
    }

    // 6)
    public decimal GetImagesCovering(
        SplitInfo info)
    {
        decimal result =
            (info.Hmax + info.TopOffset
             - info.FirstHeightMiddle
             - info.FirstStrapBottom)
            / (decimal)(info.HeightMiddle + info.Strap) + 1;
        return result;
    }

    // 7)
    public int GetImagesCount(
        SplitInfo info)
    {
        decimal result = Math.Ceiling(info.ImagesCovering);
        VerificationOfImageCount(info);
        return (int)result;
    }

    private void VerificationOfImageCount(
        SplitInfo info)
    {
        decimal verification = 
            (info.Hmax - info.FirstHeightMiddle
             - info.FirstStrapBottom - info.LastHeightMiddle
             - info.LastStrapBottom) / (info.HeightMiddle + info.Strap) + 1;
    }

    // 8) _lastSumStrapPlusHeightMiddle
    // invoked during ImagesCount setter

    // 9)
    public int GetLastStrapBottom(
        SplitInfo info)
    {
        int result = info.GetLastStrapBottom();
        return result;
    }
    
    // 10)
    public int GetLastHeightMiddle(
        SplitInfo info)
    {
        int result = info.GetLastHeightMiddle();
        return result;
    }
    
    public int GetStartPosOfHeightMiddle(
        int i, SplitInfo info)
    {
        if (i == 0)
        {
            return 0;
        }
        
        HeightInfo previous = info.HeightInfosArray[i - 1];
        int result = previous.YSHC_StartPosOfHeightCrop
                     + previous.StrapTop
                     + previous.HeightMiddle;
        return result;
    }
    
    public int GetStrapTop(
        int i, SplitInfo info)
    {
        if (i == 0)
        {
            return 0;
        }
        
        return info.Strap;
    }

    public int GetStartPosOfHeightCrop(
        int i, SplitInfo info)
    {
        if (i == 0)
        {
            return 0;
        }
        
        if (i == 1)
        {
            return info.FirstHeightMiddle;
        }
        
        int result =
            info.FirstHeightMiddle
            + (i - 1) * (info.HeightMiddle + info.Strap);
        return result;
        
        // HeightInfo previous = info.HeightInfosArray[i - 1];
        // int result = previous.YTop_StartPosOfTopHeight
        //              + previous.StrapTop
        //              + previous.HeightMiddle;
        return result;
    }

    public int GetEndPosOfHeightMiddle(int i, SplitInfo info)
    {
        int result = GetStartPosOfHeightCrop(i, info) + info.HeightCrop;
        return result;
    }
    public int GetHeightMiddle(
        int i, SplitInfo info)
    {
        if (i == 0)
        {
            return info.FirstHeightMiddle;
        }

        if (i == info.ImagesCount - 1)
        {
            return info.LastHeightMiddle;
        }
        
        return info.HeightMiddle;
    }
    
    public int GetStrapBottom(
        int i, SplitInfo info)
    {
        if (i == 0)
        {
            return info.Strap;
        }

        if (i == info.ImagesCount - 1)
        {
            return info.LastStrapBottom;
        }
        
        return info.Strap;
    }
    public HeightInfo GetHeightInfo(int i, SplitInfo info)
    {
        int strapTop = GetStrapTop(i, info);
        int heightMiddle = GetHeightMiddle(i, info);
        int diffHeightMiddle = info.HeightMiddle - heightMiddle;
        CheckDiffHeightMiddle(i, diffHeightMiddle, info);
        
        int strapBottom = GetStrapBottom(i, info);
        int heightCrop = strapTop + heightMiddle + strapBottom;
        int diffHeightCrop = info.HeightCrop - heightCrop;
        //if (i != 0){ diffHeightCrop -= 1; }
        CheckDiffHeightCrop(i, diffHeightCrop, info);
        
        int startOfHeightCrop = GetStartPosOfHeightCrop(i, info);
        int startOfMiddleHeight = startOfHeightCrop + strapTop;
        int endOfMiddleHeight = startOfMiddleHeight + heightMiddle;
        int endOfHeightCrop = endOfMiddleHeight + strapBottom;

        HeightInfo heightInfo = new(
            startOfHeightCrop,
            startOfMiddleHeight,
            endOfMiddleHeight,
            endOfHeightCrop,
            strapTop,
            strapBottom,
            heightCrop,
            heightMiddle);
        
        return heightInfo;
    }

    private void CheckDiffHeightCrop(
        int i,
        int diff,
        SplitInfo info)
    {
        bool diffEqualZero = diff == 0;
        bool isLast =
            i == info.ImagesCount - 1;
        
        if (!isLast &&
            !diffEqualZero)
        {
            //throw new Exception();
        }  
    }

    private void CheckDiffHeightMiddle(
        int i,
        int diff,
        SplitInfo info)
    {
        bool diffEqualZero = diff == 0;
        bool isFristOrLast =
            i == 0 ||
            i == info.ImagesCount - 1;
        
        if (!isFristOrLast &&
            !diffEqualZero)
        {
            //throw new Exception();
        }   
    }


    public int GetEndPosOfHeightCrop(int i, SplitInfo info)
    {
        int result = GetEndPosOfHeightMiddle(i, info) + info.Strap;
        return result;
    }
}
