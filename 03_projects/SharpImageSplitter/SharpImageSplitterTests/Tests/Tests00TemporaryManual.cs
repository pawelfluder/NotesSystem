using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpImageSplitterProg.Models;
using SharpImageSplitterTests.Examples;

namespace SharpImageSplitterTests.Tests;

[TestClass]
public class Tests00TemporaryManual
{
    private readonly ExamplesProvider _p = new();
    
    [DataRow(nameof(_p.Example_00_02))]
    [TestMethod]
    public void  TestByOnlyInfoRecalcute(
        string exampleName)
    {
        // act
        SplitInfo? info = (SplitInfo)_p.RunMethods
            .RunMethod(exampleName)!;
        
        // assert
        string gg = info.EverythingToString();
    }
    
    [DataRow(nameof(_p.Example_00_02))]
    [TestMethod]
    public void  TestByCreateImages(
        string exampleName)
    {
        // act
        SplitInfo? info = (SplitInfo)_p.RunMethods
            .RunMethod(exampleName)!;
        
        // assert
        string gg = info.EverythingToString();
    }
}
