using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpImageSplitterProg.Models;
using SharpImageSplitterTests.Examples;

namespace SharpImageSplitterTests.Tests;

[TestClass]
public class Tests05TopOffset_Real
{
    private readonly ExamplesProvider _p = new();
    
    [DataRow(nameof(_p.Example_4_2))]
    [TestMethod]
    public void  TestMethod1(
        string exampleName)
    {
        //arrange
        string expectedHeights = GetHeightsExpectedFor_4_1();
        string expectedRectanges = GetRectanglesExpectedFor(exampleName);
        
        // act5
        SplitInfo? info = (SplitInfo)_p.RunMethods
            .RunMethod(exampleName)!;
        
        // assert
        var tmp = info.EverythingToString();
        string actualHeightInfos = info.HeightInfosToString();
        List<string> diff = TestsHelper
            .ShowDiff(expectedHeights, actualHeightInfos);
        Assert.AreEqual(0, diff.Count);
        Assert.AreEqual(expectedHeights, actualHeightInfos);
        
        string actualRectangles = info.RectanglesToString();
        Assert.AreEqual(expectedRectanges, actualRectangles);
    }

    private string GetHeightsExpectedFor_4_1()
    {
        return
            """
            HInfo [ Y_SHC=_0; Y_SHM=_0; Y_EHM=_4; Y_EHC=_9; ST=0; SB=5; HM=_4; HC=_9; ]
            HInfo [ Y_SHC=_4; Y_SHM=_9; Y_EHM=19; Y_EHC=24; ST=5; SB=5; HM=10; HC=20; ]
            HInfo [ Y_SHC=19; Y_SHM=24; Y_EHM=34; Y_EHC=39; ST=5; SB=5; HM=10; HC=20; ]
            HInfo [ Y_SHC=34; Y_SHM=39; Y_EHM=49; Y_EHC=54; ST=5; SB=5; HM=10; HC=20; ]
            HInfo [ Y_SHC=49; Y_SHM=54; Y_EHM=55; Y_EHC=55; ST=5; SB=0; HM=_1; HC=_6; ]
            """;
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
    
    private string GetExpectedFor_4_2()
    {
        return
            """
            HInfo [ Y_SHC=____0; Y_SHM=____0; Y_EHM=-1979; Y_EHC=-1954; ST=_0; SB=25; HM=-1979; HC=-1954; ]
            HInfo [ Y_SHC=-1979; Y_SHM=-1954; Y_EHM=__541; Y_EHC=__566; ST=25; SB=25; HM=_2495; HC=_2545; ]
            HInfo [ Y_SHC=__541; Y_SHM=__566; Y_EHM=_3061; Y_EHC=_3086; ST=25; SB=25; HM=_2495; HC=_2545; ]
            HInfo [ Y_SHC=_3061; Y_SHM=_3086; Y_EHM=_5581; Y_EHC=_5606; ST=25; SB=25; HM=_2495; HC=_2545; ]
            HInfo [ Y_SHC=_5581; Y_SHM=_5606; Y_EHM=_8101; Y_EHC=_8126; ST=25; SB=25; HM=_2495; HC=_2545; ]
            HInfo [ Y_SHC=_8101; Y_SHM=_8126; Y_EHM=10621; Y_EHC=10646; ST=25; SB=25; HM=_2495; HC=_2545; ]
            HInfo [ Y_SHC=10621; Y_SHM=10646; Y_EHM=13141; Y_EHC=13166; ST=25; SB=25; HM=_2495; HC=_2545; ]
            HInfo [ Y_SHC=13141; Y_SHM=13166; Y_EHM=_9171; Y_EHC=_9171; ST=25; SB=_0; HM=-3995; HC=-3970; ]
            """;
    }
}
