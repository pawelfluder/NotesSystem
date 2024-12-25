// using System;
// using System.Collections.Generic;
// using SharpImageSplitterProg.Models;
// using SixLabors.ImageSharp;
//
// namespace SharpImageSplitterProg.Backup.Workers;
//
// internal class RecalculateJob2
// { 
//     private SplitInfo _info { get; set; }
//     
//     public SplitInfo Do(
//         double heightByWidthRatio,
//         double overlapPercentage,
//         int imageWidth,
//         int imageHeight,
//         int topOffset)
//     {
//         
//         _info = new SplitInfo();
//         
//         _info.HWRatio = heightByWidthRatio;
//         _info.Olap = overlapPercentage / 100;
//         _info.Wmax = imageWidth;
//         _info.Hmax = imageHeight;
//         _info.CenterBigHeight = (int)(_info.HWRatio * _info.Wmax);
//         _info.FirstSmallHeight = (int)(_info.CenterBigHeight * (1-_info.Olap));
//         _info.CenterSmallHeight = (int)(_info.CenterBigHeight * (1-2*_info.Olap));
//         _info.Strap = (int)(_info.CenterBigHeight * _info.Olap);
//         _info.ImagesCount = GetImagesCount();
//         _info.HeightInfosArray = new HeightInfo[_info.ImagesCount];
//         _info.LastStrapBottom = GetLastImageSmallHeight();
//         
//         for (int i = 0; i < _info.ImagesCount; i++)
//         {
//             AddToPositionHeightArray(i);
//             AddToPositionIntDict(i);
//         }
//
//         return _info;
//     }
//
//     private void AddToPositionIntDict(int i)
//     {
//         int smallHeightPos = GetSmallHeightPos(i);
//         int bigHeightPos = GetBigHeightPos(i);
//         int cropHeight = GetCropHeight(i);
//         _info.PositionsDict.Add(i, [smallHeightPos, bigHeightPos, cropHeight]);
//     }
//     
//     private void AddToPositionHeightArray(int i)
//     {
//         int startOfTopHeight = GetSmallHeightPos(i);
//         int startOfMiddleHeight = GetBigHeightPos(i);
//         int startOfBottomHeight = GetCropHeight(i);
//         int endHeight = startOfBottomHeight + _info.Strap;
//         int topStrap = startOfMiddleHeight - startOfTopHeight;
//         int bottomStrap = startOfBottomHeight - endHeight;
//         int rectangleHeight = endHeight - startOfTopHeight;
//         int middleHeight = startOfBottomHeight - startOfMiddleHeight;
//         HeightInfo heightInfo = new(
//             startOfTopHeight,
//             startOfMiddleHeight,
//             startOfBottomHeight,
//             endHeight,
//             topStrap,
//             bottomStrap,
//             rectangleHeight,
//             middleHeight);
//         _info.HeightInfosArray[i] = heightInfo;
//     }
//     
//     private int GetSmallHeightPos(int i)
//     {
//         if (i == 0)
//         {
//             return 0;
//         }
//
//         if (i == 1)
//         {
//             return _info.FirstSmallHeight;
//         }
//
//         if (i == _info.ImagesCount - 1)
//         {
//             return _info.LastStrapBottom;
//         }
//             
//         int result = _info.FirstSmallHeight + _info.CenterSmallHeight * (i-1);
//         return result;
//     }
//
//     private int GetBigHeightPos(int i)
//     {
//         if (i == 0)
//         {
//             return 0;
//         }
//
//         int result = _info.CenterSmallHeight * i;
//         return result;
//     }
//
//     private int GetCropHeight(int i)
//     {
//         if (i == 0)
//         {
//             int result = _info.FirstSmallHeight + _info.Strap;
//             return result;
//         }
//
//         if (i == _info.ImagesCount - 1)
//         {
//             int result = _info.LastStrapBottom + _info.Strap;
//             return result;
//         }
//             
//         return _info.CenterBigHeight;
//     }
//
//     private int GetImagesCount()
//     {
//         double tmp = (_info.Hmax - _info.FirstSmallHeight) / (double)_info.CenterSmallHeight;
//         int count = (int)Math.Floor(tmp) + 1;
//         int rest = (int)(_info.Hmax - _info.FirstSmallHeight - (count-1) * _info.CenterSmallHeight);
//
//         if (rest <= _info.Strap)
//         {
//             return count;
//         }
//
//         return count + 1;
//     }
//
//     private int GetLastImageSmallHeight()
//     {
//         int strap = (int)(_info.CenterBigHeight * _info.Olap);
//         int tmp = _info.FirstSmallHeight + (_info.ImagesCount - 2) * _info.CenterSmallHeight;
//         int rest = _info.Hmax - tmp;
//         
//         return rest;
//     }
//
//     public Rectangle RectangleByPositionHeighArray(int i)
//     {
//         int smallHeightPos = _info.PositionsDict[i][0];
//         int bigHeightPos = _info.PositionsDict[i][1];
//         int cropHeight = _info.PositionsDict[i][2];
//         Rectangle rectangle = new(
//             0,
//             bigHeightPos,
//             _info.Wmax,
//             cropHeight);
//         return rectangle;
//     }
//
//     public Rectangle RectangleByPositionIntDict(int i)
//     {
//         HeightInfo array = _info.HeightInfosArray[i];
//         int smallHeightPos = array.YTop_StartPosOfTopHeight;
//         int bigHeightPos = array.YMiddle_StartPosOfMiddleHeight;
//         int cropHeight = array.YBottom_StartPosOfBottomHeight + _info.Strap;
//         Rectangle rectangle = new(
//             0,
//             bigHeightPos,
//             _info.Wmax,
//             cropHeight);
//         return rectangle;
//     }
// }