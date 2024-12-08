// using System;
// using SharpImageSplitterProg.Models;
//
// namespace SharpImageSplitterProg.Workers;
//
// internal class HeightInfoEngineJob2
// {
//     public int GetSmallHeightPos(
//         int i, SplitInfo info)
//     {
//         if (i == 0)
//         {
//             return 0;
//         }
//
//         if (i == 1)
//         {
//             return info.FirstMiddleHeight;
//         }
//
//         if (i == info.ImagesCount - 1)
//         {
//             return info.LastStrapBottom;
//         }
//             
//         int result = info.FirstMiddleHeight + info.HeightMiddle * (i-1);
//         return result;
//     }
//
//     public int GetBigHeightPos(
//         int i, SplitInfo info)
//     {
//         if (i == 0)
//         {
//             return 0;
//         }
//
//         int result = info.HeightMiddle * i;
//         return result;
//     }
//
//     public int GetCropHeight(
//         int i, SplitInfo info)
//     {
//         if (i == 0)
//         {
//             int result = info.FirstMiddleHeight + info.Strap - info.TopOffset;
//             return result;
//         }
//
//         if (i == info.ImagesCount - 1)
//         {
//             int result = info.LastStrapBottom + info.Strap;
//             return result;
//         }
//             
//         return info.HeightCrop;
//     }
//
//     public int GetImagesCount(
//         SplitInfo info)
//     {
//         double tmp = (info.Hmax - info.FirstMiddleHeight) / (double)info.HeightMiddle;
//         int count = (int)Math.Floor(tmp) + 1;
//         int rest = (int)(info.Hmax - info.FirstMiddleHeight - (count-1) * info.HeightMiddle);
//
//         if (rest <= info.Strap)
//         {
//             return count;
//         }
//
//         return count + 1;
//     }
//
//     public int GetLastStrapBottom(
//         SplitInfo info)
//     {
//         int strap = (int)(info.HeightCrop * info.Olap);
//         int tmp = info.FirstMiddleHeight + (info.ImagesCount - 2) * info.HeightMiddle;
//         int rest = info.Hmax - tmp;
//         
//         return rest;
//     }
//
//     public int GetCropHeight(SplitInfo info)
//     {
//         int result = (int)(info.HWRatio * info.Wmax);
//         return result;
//     }
//
//     public int GetFirstStrapTop(SplitInfo info)
//     {
//         int result = (int)(info.HeightCrop * (1-info.Olap));
//         return result;
//     }
//
//     public int GetStrap(SplitInfo info)
//     {
//         int result = (int)(info.HeightCrop * info.Olap);
//         return result;
//     }
//
//     public int GetMiddleHeight(SplitInfo info)
//     {
//         double result = info.HeightCrop * (1-2*info.Olap);
//         return (int)result;
//     }
// }
