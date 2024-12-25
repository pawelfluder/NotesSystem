// using System;
// using System.Collections.Generic;
// using System.IO;
// using SharpImageSplitterProg.AAPublic.Strategies;
// using SharpOperationsProg.AAPublic;
// using SixLabors.ImageSharp;
// using SixLabors.ImageSharp.Processing;
//
// namespace SharpImageSplitterProg.Backup.Workers;
//
// public class SplitterJob2
// {
//     private readonly StrategyBase<ISplitStrategy> _strategyBase;
//
//     public SplitterJob2()
//     {
//         _strategyBase = new StrategyBase<ISplitStrategy>();
//     }
//
//     // max
//     public int Hmax {get; private set;}
//     public int Wmax {get; private set;}
//
//     // first
//     public int FirstSmallHeight {get; private set;}
//
//     // center
//     public int CenterBigHeight {get; private set;}
//     public int CenterSmallHeight {get; private set;}
//
//     /// last
//     public int LastSmallHeight {get; private set;}
//
//     // input
//     public double Olap {get; private set;}
//     public double HWRatio {get; private set;}
//         
//     // rest
//     public int Strap {get; private set;}
//     public int ImagesCount {get; private set;}
//
//     public Dictionary<int, int[]> PositionsDict {get; private set;}
//
//     public void ReCalculate(
//         double heightByWidthRatio,
//         double overlapPercentage,
//         int imageWidth,
//         int imageHeight,
//         int topOffset)
//     {
//         HWRatio = heightByWidthRatio;
//         Olap = overlapPercentage / 100;
//         Wmax = imageWidth;
//         Hmax = imageHeight;
//         CenterBigHeight = (int)(HWRatio * Wmax);
//         FirstSmallHeight = (int)(CenterBigHeight * (1-Olap));
//         CenterSmallHeight = (int)(CenterBigHeight * (1-2*Olap));
//         Strap = (int)(CenterBigHeight * Olap);
//         ImagesCount = GetImagesCount();
//         LastSmallHeight = GetLastImageSmallHeight();
//
//         PositionsDict = new Dictionary<int, int[]>();
//         for (int i = 0; i < ImagesCount; i++)
//         {
//             int smallHeightPos = GetSmallHeightPos(i);
//             int bigHeightPos = GetBigHeightPos(i);
//             int cropHeight = GetCropHeight(i);
//             PositionsDict.Add(i, new[] { smallHeightPos, bigHeightPos, cropHeight});
//         }
//     }
//
//     public void CreateSplitImages(
//         (string folderPath, string fileName) folderQfile,
//         string splitStrategyName)
//     {
//         ISplitStrategy strategy = _strategyBase.GetNewStrategy(splitStrategyName);
//         CreateSplitImages(folderQfile, strategy.HeightByWidth, strategy.Overlap);
//     }
//
//     internal string GetImageFilePath(
//         (string folderPath, string fileName) folderQfile)
//     {
//         string filePath = Path.Combine(folderQfile.folderPath, folderQfile.fileName);
//         filePath = filePath.Replace("\\", "/");
//         if (!File.Exists(filePath))
//         {
//             throw new FileNotFoundException("File not found", filePath);
//         }
//         return filePath;
//     }
//
//     internal string CreateTempFolder(
//         (string folderPath, string fileName) folderQfile)
//     {
//         string tempFolderPath = Path.Combine(folderQfile.folderPath, "temp");
//
//         if (!Directory.Exists(tempFolderPath))
//         {
//             Directory.CreateDirectory(tempFolderPath);
//         }
//         return tempFolderPath;
//     }
//
//     internal string CreateSplitImages(
//         (string folderPath, string fileName) folderQfile,
//         decimal heightByWidthRatio,
//         decimal overlapPercentage)
//     {
//         string tempFolderPath = CreateTempFolder(folderQfile);
//         string filePath = Path.Combine(folderQfile.folderPath, folderQfile.fileName);
//         filePath = filePath.Replace("\\", "/");
//         
//         Image image = Image.Load(filePath);
//         string result = CreateSplitImages(
//             image,
//             heightByWidthRatio,
//             overlapPercentage,
//             tempFolderPath);
//         return result;
//     }
//
//     internal string CreateSplitImages(
//         Image image,
//         decimal heightByWidthRatio,
//         decimal overlapPercentage,
//         string tempFolderPath)
//     {
//         ReCalculate(heightByWidthRatio, overlapPercentage, image.Width, image.Height, 200);
//
//         for (int i = 0; i < ImagesCount; i++)
//         {
//             var smallHeightPos = PositionsDict[i][0];
//             var bigHeightPos = PositionsDict[i][1];
//             var cropHeight = PositionsDict[i][2];
//             var cropRectangle = new Rectangle(
//                 0,
//                 bigHeightPos,
//                 image.Width,
//                 cropHeight);
//                 
//             var name = $"{IndexToString(i+1)}_item.png";
//             var outputfilePath = Path.Combine(tempFolderPath, name);
//
//             try
//             {
//                 var clonedImage = image.Clone(ctx => ctx.Crop(cropRectangle));
//                 clonedImage.Save(outputfilePath);
//             }
//             catch
//             {
//                 // ignored
//             }
//         }
//             
//         image.Dispose();
//         return tempFolderPath;
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
//             return FirstSmallHeight;
//         }
//
//         if (i == ImagesCount - 1)
//         {
//             return LastSmallHeight;
//         }
//             
//         var result = FirstSmallHeight + CenterSmallHeight * (i-1);
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
//         var result = CenterSmallHeight * i;
//         return result;
//     }
//
//     private int GetCropHeight(int i)
//     {
//         if (i == 0)
//         {
//             var result = FirstSmallHeight + Strap;
//             return result;
//         }
//
//         if (i == ImagesCount - 1)
//         {
//             var result = LastSmallHeight + Strap;
//             return result;
//         }
//             
//         return CenterBigHeight;
//     }
//
//     private int GetImagesCount()
//     {
//         var tmp = (Hmax - FirstSmallHeight) / (double)CenterSmallHeight;
//         var count = (int)Math.Floor(tmp) + 1;
//         var rest = (int)(Hmax - FirstSmallHeight - (count-1) * CenterSmallHeight);
//
//         if (rest <= Strap)
//         {
//             return count;
//         }
//
//         return count + 1;
//     }
//
//     private int GetLastImageSmallHeight()
//     {
//         var strap = (int)(CenterBigHeight * Olap);
//         var tmp = FirstSmallHeight + (ImagesCount - 2) * CenterSmallHeight;
//         var rest = Hmax - tmp;
//                 
//         // var tmp = (Hmax - this.FirstSmallHeight) / (double)this.CenterSmallHeight;
//         // var count = (int)Math.Floor(tmp) + 1;
//         // var rest = (int)(Hmax - this.FirstSmallHeight - (count-1) * CenterSmallHeight);
//
//         return rest;
//             
//         // if (rest < 2 * strap
//         //     && rest >= 1)
//         // {
//         //     var result = rest - strap;
//         //     return result;
//         // }
//             
//         // if (rest + strap <= CenterBigHeight)
//         // {
//         // }
//     }
//
//     private string IndexToString(int index)
//     {
//         if (index < 10)
//         {
//             return "0" + index;
//         }
//         if (index < 100)
//         {
//             return index.ToString();
//         }
//
//         throw new Exception();
//     }
//
//     // wyjaśnienie
//     // kazdy srodkowy obrazek ma smallHeight + 2*strap
//     // pierwszy obrazek ma 1*strap 
//     // ostatni obrazek ma 1*strap + smallHeight które jest mniejsze
// }