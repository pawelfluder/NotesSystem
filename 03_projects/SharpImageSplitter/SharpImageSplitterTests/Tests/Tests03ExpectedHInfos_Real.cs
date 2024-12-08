using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpImageSplitterProg.Models;
using SharpImageSplitterTests.Examples;

namespace SharpImageSplitterTests.Tests;

[TestClass]
public class Tests03ExpectedHInfos_Real
{
    private readonly ExamplesProvider _p = new();
    
    [DataRow(nameof(_p.Example_3_1))]
    [TestMethod]
    public void TestMethod1(string exampleName)
    {
        // arrange
        string expectedHeights = GetHeightsExpectedFor(exampleName);
        string expectedRectanges = GetRectanglesExpectedFor(exampleName);
        
        // act
        SplitInfo? info = (SplitInfo)_p.RunMethods
            .RunMethod(exampleName)!;
        
        // assert
        string actualHeightInfos = info.HeightInfosToString();
        List<string> diff = TestsHelper
            .ShowDiff(expectedHeights, actualHeightInfos);
        Assert.AreEqual(0, diff.Count);
        Assert.AreEqual(expectedHeights, actualHeightInfos);
        
        string actualRectangles = info.RectanglesToString();
        Assert.AreEqual(expectedRectanges, actualRectangles);
    }
    
    private string GetRectanglesExpectedFor(
        string name)
    {
        if (nameof(_p.Example_3_1) == name)
        {
            return
                """
                Rectangle [ X=0, Y=2546, Width=1744, Height=2546 ]
                Rectangle [ X=0, Y=5066, Width=1744, Height=2545 ]
                Rectangle [ X=0, Y=7586, Width=1744, Height=2545 ]
                Rectangle [ X=0, Y=9171, Width=1744, Height=1610 ]
                """;
        }
        
        return string.Empty;
    }

    private string GetHeightsExpectedFor(
        string name)
    {
        // todo
        // dalej problem z HC=2546 roznica o 1 - 2495
        if (nameof(_p.Example_3_1) == name)
        {
            return
            """
            HInfo [ Y_SHC=___0; Y_SHM=___0; Y_EHM=2521; Y_EHC=2546; ST=_0; SB=25; HM=2521; HC=2546; ]
            HInfo [ Y_SHC=2521; Y_SHM=2546; Y_EHM=5041; Y_EHC=5066; ST=25; SB=25; HM=2495; HC=2545; ]
            HInfo [ Y_SHC=5041; Y_SHM=5066; Y_EHM=7561; Y_EHC=7586; ST=25; SB=25; HM=2495; HC=2545; ]
            HInfo [ Y_SHC=7561; Y_SHM=7586; Y_EHM=9171; Y_EHC=9171; ST=25; SB=_0; HM=1585; HC=1610; ]
            """;
        }

        return string.Empty;
    }
}
