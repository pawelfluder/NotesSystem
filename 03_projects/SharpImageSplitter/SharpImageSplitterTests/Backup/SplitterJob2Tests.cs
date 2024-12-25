// using System.IO;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using SharpImageSplitterProg.AAPublic.Names;
// using SharpImageSplitterProg.AAPublic.Strategies;
// using SharpImageSplitterProg.Backup.Workers;
// using SharpImageSplitterProg.Service;
// using SharpImageSplitterTests.Registration;
// using SharpOperationsProg.AAPublic;
// using SharpOperationsProg.AAPublic.Operations;
// using SixLabors.ImageSharp;
// using SixLabors.ImageSharp.Formats;
// using OutBorder01 = SharpSetup21ProgPrivate.AAPublic.OutBorder;
//
// namespace SharpImageSplitterTests.Backup;
//
// [TestClass]
// public class SplitterJob2Tests
// {
//     private readonly IOperationsService _operations;
//     private readonly StrategyBase<ISplitStrategy> _strategyBase;
//     private readonly ImageSpliterService _service;
//
//     public SplitterJob2Tests()
//     {
//         OutBorder01.GetPreparer("PrivateNotesPreparer").Prepare();
//         _operations = MyBorder.Container.Resolve<IOperationsService>();
//         _service = new ImageSpliterService();
//         _strategyBase = new StrategyBase<ISplitStrategy>();
//     }
//     
//     [TestMethod]
//     public void TestMethod1()
//     {
//         // arrange
//         Stream imageStream = IEmbeddedResourcesOperations
//             .GetEmbeddedResourceStream(this,"01_item.png");
//         DecoderOptions options = new();
//         Image image = Image.Load(options, imageStream);
//         SplitterJob2 splitterJob = new SplitterJob2();
//         string tempFolderPath = _operations.Path.GetBinPath() + "/" + "temp";
//         
//         // act
//         string splitStrategyName = IResizeStrategyNames.FullPage;
//         ISplitStrategy strategy = _strategyBase
//             .GetNewStrategy(splitStrategyName);
//         splitterJob.CreateSplitImages(
//             image,
//             strategy.HeightByWidth,
//             strategy.Overlap,
//             tempFolderPath);
//         
//         // assert
//         bool b1 = HmaxEqualSum(splitterJob);
//         Assert.IsTrue(b1);
//     }
//         
//     [DataRow(50, 550, 2.0, 5.0)]
//     // 50x500, 5%, h/w=2=2/1=100/50
//     // 95 + 4*90 + 45 = 500
//     // 100 + 4*100 + 50 = 550
//     // 1:95i5 2,3,4:5i90i5 5:5i45
//
//     [DataRow(50, 551, 2.0, 5.0)]
//     // 50x551, 5%, h/w=2=2/1=100/50
//     // 95 + 5*90 + 6 = 551
//     // 100 + 5*100 + 11 = 611
//     // 1:95i5 2,3,4:5i90i5 5:5i94i1 6:5i1
//         
//     [DataRow(50, 551, 2.0, 5.0)]
//     // 50x555, 5%, h/w=2=2/1=100/50
//     // 95 + 5*90 + 10 = 555
//     // 100 + 5*100 + 15 = 615
//     // 1:95i5 2,3,4,5,6:5i90i5 5:5i94i1 6:5i1
//     [TestMethod]
//     public void TestMethod1(
//         int width,
//         int height,
//         double hwRatio,
//         double olap)
//     {
//         // arrange
//         var splitter = new SplitterJob2();
//             
//         // act
//         splitter.ReCalculate(hwRatio, olap, width, height, 0);
//             
//         // assert
//         var b1 = HmaxEqualSum(splitter);
//         Assert.IsTrue(b1);
//     }
//
//     private bool HmaxEqualSum(SplitterJob2 sp)
//     {
//         int sum = sp.FirstSmallHeight + (sp.ImagesCount-2) * sp.CenterSmallHeight + sp.LastSmallHeight;
//         bool correct = sp.Hmax == sum;
//         return correct;
//     }
// }