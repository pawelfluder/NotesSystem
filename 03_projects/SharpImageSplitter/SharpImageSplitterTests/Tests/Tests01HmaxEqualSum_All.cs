using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpImageSplitterProg.Models;
using SharpImageSplitterTests.Examples;

namespace SharpImageSplitterTests.Tests;

[TestClass]
public class Tests01HmaxEqualSum_All
{
    private readonly ExamplesProvider _p = new();

    [DataRow(nameof(_p.Example_1_1))]
    [DataRow(nameof(_p.Example_1_2))]
    [DataRow(nameof(_p.Example_2_1))]
    [DataRow(nameof(_p.Example_2_2))]
    [DataRow(nameof(_p.Example_3_1))]
    [DataRow(nameof(_p.Example_4_1))]
    [TestMethod]
    public void  TestMethod1(
        string exampleName)
    {
        // act
        SplitInfo? info = (SplitInfo)_p.RunMethods
            .RunMethod(exampleName)!;
        
        // assert
        bool correct1 = HmaxEqualSum(info);
        Assert.IsTrue(correct1);
    }
    
    private bool HmaxEqualSum(SplitInfo sp)
    {
        int sum =
            sp.FirstHeightMiddle
            + sp.FirstStrapBottom
            + (sp.ImagesCount - 2) * (sp.HeightMiddle + sp.Strap)
            + sp.LastHeightMiddle
            + sp.LastStrapBottom;
        bool correct = sp.Hmax == sum;
        return correct;
    }
}
