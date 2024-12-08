using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpImageSplitterProg.Models;
using SharpImageSplitterTests.Examples;

namespace SharpImageSplitterTests.Tests;

[TestClass]
public class Tests02ExpectedHInfos_Simple
{
    private readonly ExamplesProvider _p = new();
    
    [DataRow(nameof(_p.Example_1_1))]
    [DataRow(nameof(_p.Example_1_2))]
    [DataRow(nameof(_p.Example_2_1))]
    [DataRow(nameof(_p.Example_2_2))]
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
        List<string> diff = TestsHelper.
            ShowDiff(expectedHeights, actualHeightInfos);
        Assert.AreEqual(0, diff.Count);
        Assert.AreEqual(expectedHeights, actualHeightInfos);
        
        string actualRectangles = info.RectanglesToString();
        Assert.AreEqual(expectedRectanges, actualRectangles);
    }
    
    private string GetRectanglesExpectedFor(
        string name)
    {
        return string.Empty;
    }

    private string GetHeightsExpectedFor(
        string name)
    {
        return string.Empty;
    }
}
