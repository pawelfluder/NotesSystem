using SharpImageSplitterProg.AAPublic.Names;
using SharpImageSplitterProg.AAPublic.Strategies;
using SharpImageSplitterProg.Models;
using SharpImageSplitterProg.Workers;
using SharpImageSplitterTests.Registration;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using OutBorder01 = SharpSetup01Prog.AAPublic.OutBorder;

namespace SharpImageSplitterTests.Examples;

internal partial class ExamplesProvider
{
    private readonly RecalculateJob _recalculate;
    public RunMethodsBase RunMethods { get; private set; }
    
    private readonly StrategyBase<ISplitStrategy> _strategyBase;
    private readonly IOperationsService _operations;
    private readonly string _binDebugNet80Path;

    public ExamplesProvider()
    {
        _recalculate = new RecalculateJob();
        RunMethods = new RunMethodsBase(this);
        _strategyBase = new StrategyBase<ISplitStrategy>();
        OutBorder01.GetPreparer("PrivateNotesPreparer").Prepare();
        _operations = MyBorder.OutContainer.Resolve<IOperationsService>();
        _binDebugNet80Path = _operations.Path.GetBinPath() + "/" + "/Debug/net8.0/";
    }
    
    public SplitInfo Example_1_1()
    {
        SplitInfo info = _recalculate.Do(
            2.0m,
            25.0m,
            10, 
            54,
            0);
        return info;
        // double HeightByWidthRatio = 2.0
        // double OverlapPercentage = 25
        // int ImageWidth = 10
        // int ImageHeight = 54
        // int TopOffset = 0
    }
    
    public SplitInfo Example_1_2()
    {
        SplitInfo info = _recalculate.Do(
            2.0m,
            25.0m,
            10, 
            69,
            0);
        return info;
        // double HeightByWidthRatio = 2.0
        // double OverlapPercentage = 25
        // int ImageWidth = 10
        // int ImageHeight = 69
        // int TopOffset = 0
    }
    
    public SplitInfo Example_2_1()
    {
        SplitInfo info = _recalculate.Do(
             2.0m, 5.0m, 50, 550);
        return info;
        // h/w=2, 5%, 50x500,
        // h/w=2=2/1=100/50
        // 95 + 4*90 + 45 = 500 (sum of all middle height)
        // 1:95i5 2,3,4:5i90i5 5:5i45
        // 100 + 4*100 + 50 = 550 (sum of all crop height)
    }
    
    public SplitInfo Example_2_2()
    {
        SplitInfo info = _recalculate.Do(
            2.0m, 5.0m, 50, 551);
        return info;
        //[DataRow(50, 551, 2.0, 5.0)]
        // 50x551, 5%, h/w=2=2/1=100/50
        // 95 + 5*90 + 6 = 551
        // 100 + 5*100 + 11 = 611
        // 1:95i5 2,3,4:5i90i5 5:5i94i1 6:5i1
    }
    
    public SplitInfo Example_3_1()
    {
        // arrange
        SplitterJob splitterJob = new SplitterJob();
        (string, string) folderQfile = (
            _binDebugNet80Path,
            "01_item.png");
        
        // act
        string splitStrategyName = IResizeStrategyNames.FullPageStrategy;
        ISplitStrategy strategy = _strategyBase
            .GetNewStrategy(splitStrategyName);
        SplitInfo info = splitterJob.CreateSplitImages(
            folderQfile,
            strategy.HeightByWidth,
            strategy.Overlap);
        return info;
    }
    
    public SplitInfo Example_4_1()
    {
        SplitInfo info = _recalculate.Do(
            2.0m,
            25.0m,
            10, 
            55,
            11);
        return info;
    }
    
    public SplitInfo Example_4_2()
    {
        // arrange
        SplitterJob splitterJob = new SplitterJob();
        (string, string) folderQfile = (
            _binDebugNet80Path,
            "01_item.png");
        
        // act
        string splitStrategyName = IResizeStrategyNames.FullPageStrategy;
        ISplitStrategy strategy = _strategyBase
            .GetNewStrategy(splitStrategyName);
        SplitInfo info = splitterJob.CreateSplitImages(
            folderQfile,
            strategy.HeightByWidth,
            strategy.Overlap,
            1000);
        return info;
    }
}
