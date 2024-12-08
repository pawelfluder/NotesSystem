using System;
using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp;

namespace SharpImageSplitterProg.Models;

internal class SplitInfo
{
    string _newLine = "\r\n";
    
    // paths
    public string TempFolderPath { get; set; }
    public string ImageFilePath { get; set; }
    
    // infos
    public HeightInfo[] HeightInfosArray { get; set; }
    public Rectangle[] RectanglesArray { get; set; }
    
    // inputs
    public decimal HWRatio { get; set; }
    public decimal Olap { get; set; }
    public int Wmax { get; set; }
    public int Hmax { get; set; }
    public int TopOffset { get; set; }
    
    // calculated
    public int HeightCrop { get; set; } // 1)
    public int HeightMiddle { get; set; } // 2)
    public int Strap { get; set; } // 3)
    public int FirstHeightMiddle { get; set; } // 4)
    public int FirstStrapBottom { get; set; } // 5)
    
    public decimal ImagesCovering { get; set; } // 6)
    
    private int _imagesCount; // 7)

    private int _lastSumStrapAndHeightMiddle; // 8)
    public int LastStrapBottom { get; set; } // 8)
    public int LastHeightMiddle { get; set; } // 9)
    public int ImagesCount
    {
        get => _imagesCount;
        set
        {
            _imagesCount = value;
            RectanglesArray = new Rectangle[_imagesCount];
            HeightInfosArray = new HeightInfo[_imagesCount];
            SetLastSumStrapPlusHeightMiddle(this);
        }
    }

    private void SetLastSumStrapPlusHeightMiddle(
        SplitInfo info)
    {
        int partA =
            info.Hmax
            - info.FirstHeightMiddle
            - info.FirstStrapBottom;
        int partB =
            (ImagesCount - 2)
            * (info.HeightMiddle + info.FirstStrapBottom);
        int result = partA - partB;
        _lastSumStrapAndHeightMiddle = result;
    }
    
    public int GetLastStrapBottom()
    {
        bool isGrater = _lastSumStrapAndHeightMiddle > HeightMiddle;
        if (isGrater)
        {
            int result = _lastSumStrapAndHeightMiddle - HeightMiddle;
            return result;
        }

        return 0;
    }

    public int GetLastHeightMiddle()
    {
        int result = _lastSumStrapAndHeightMiddle - LastStrapBottom;
        return result;
    }

    public string RectanglesToString()
    {
        string newLine = "\r\n";
        string[] stringsArray = RectanglesArray
            .Select(x => x.ToString())
            .ToArray();
        string result = string.Join(newLine, stringsArray);
        return result;
    }

    public string EverythingToString()
    {
        var result =
            InputToString()
            + _newLine + _newLine
            + SplitInfoToString()
            + _newLine + _newLine
            + HeightInfosToString()
            + _newLine + _newLine
            + RectanglesToString()
            + _newLine;
        return result;

    }

    public string InputToString()
    {
        var result =
        $"""
        HWRatio = {HWRatio}
        ___Olap = {Olap}
        ___Hmax = {Hmax}
        ___Wmax = {Wmax}
        """;
        return result;
    }

    public string SplitInfoToString()
    {
        var result =
        $"""
        1) _______HeightCrop = {HeightCrop}
        2) _____HeightMiddle = {HeightMiddle}
        3) ____________Strap = {Strap}
        4) FirstHeightMiddle = {FirstHeightMiddle}
        5) _FirstStrapBottom = {FirstStrapBottom}
        6) ___ImagesCovering = {ImagesCovering}
        7) ______ImagesCount = {ImagesCount}
        8) __________lastSum = {_lastSumStrapAndHeightMiddle}
        9) __LastStrapBottom = {LastStrapBottom}
        10) LastHeightMiddle = {LastHeightMiddle}
        """;
        return result;
    }

    public string HeightInfosToString()
    {
        HeightInfo[] infosArray = HeightInfosArray;
        
        string newLine = "\r\n";
        string[] stringsArray = infosArray
            .Select(x => x.ToString()).ToArray();

        var equalIndexes = stringsArray
            .Select(x => FindMultipleAccurence('=',x))
            .ToArray();
        var semicolonIndexes = stringsArray
            .Select(x => FindMultipleAccurence(';',x))
            .ToArray();

        if (equalIndexes.Length != semicolonIndexes.Length)
        {
            throw new Exception();
        }

        int[][] diffArray = GetDiffArray2D(equalIndexes, semicolonIndexes);
        int[] maxArray = GetMaxArray(diffArray);
        
        int lenI = diffArray[0].Length;
        int lenJ = diffArray.Length;

        for (int j = 0; j < lenJ; j++)
        {
            var added = 0;
            for (int i = 0; i < lenI; i++)
            {
                var diff = diffArray[j][i];
                
                if (diff > 0)
                {
                    var insertPosition = equalIndexes[j][i] + added + 1;
                    var max = maxArray[i];
                    var count = max - diff;
                    var insertString = string.Join("",Enumerable.Repeat('_', count));
                    var newValue = stringsArray[j]
                        .Insert(insertPosition, insertString);
                    stringsArray[j] = newValue;
                    added += count;
                }
            }
        }
        
        string result = string.Join(newLine, stringsArray);
        return result;
    }

    private int[] GetMaxArray(int[][] diffArray)
    {
        var lenI = diffArray[0].Length;
        var lenJ = diffArray.Length;
        var maxArray = new int[lenI];
        for (int i = 0; i < lenI; i++)
        {
            var max = 0;
            for (int j = 0; j < lenJ; j++)
            {
                var current = diffArray[j][i];
                if (current > max)
                {
                    max = current;
                }
            }
            maxArray[i] = max;
        }

        return maxArray;
    }

    private int[][] GetDiffArray2D(
        int[][] equalIndexes,
        int[][] semicolonIndexes)
    {
        var lenI = equalIndexes[0].Length;
        var lenJ = equalIndexes.Length;
        var diffArray2D = new int[equalIndexes.Length][];
        for (int j = 0; j < lenJ; j++)
        {
            var currentEqualIndexes = equalIndexes[j];
            diffArray2D[j] = new int[lenI];
            for (int i = 0; i < lenI; i++)
            {
                var gg3 = equalIndexes[j][i];
                var gg4 = semicolonIndexes[j][i];
                var diff = gg4 - gg3;
                
                diffArray2D[j][i] = diff;
            }
        }

        return diffArray2D;
    }

    private int[] FindMultipleAccurence(
        char c, string inputStr)
    {
        List<int> foundIndexes = new List<int>();

        for (int i = 0; i < inputStr.Length; i++)
        {
            if (inputStr[i] == c) foundIndexes.Add(i);
            
        }
        return foundIndexes.ToArray();
    }
}
