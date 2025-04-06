using SharpImageSplitterProg.AAPublic.Names;
using SharpImageSplitterProg.AAPublic.Strategies;
using SharpImageSplitterProg.Models;
using SharpImageSplitterProg.Workers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace SharpImageSplitterTests.Examples;

internal partial class ExamplesProvider
{
    //string folderPath = @"/Volumes/qnap/01_files_refrenced/2024/03_files_photos/13_scam_krypto/05_Zespół-Binance-analizuje-formularz-zgłoszeniowy-oraz-potwierdzenie-oszustwo";
    //string filename = "Binance_team.PNG";
    
    public SplitInfo Example_00_02()
    {
        // arrange
        //string folderPath = "/Volumes/qnap/01_files_refrenced/2024/03_files_photos/13_scam_krypto/04_Formularz-zgłoszeniowy-Binance/02_raport";
        string folderPath = "/Users/pawelfluder/03_synch/02_files_refrenced/01_files_photos/gotowe";
        string filename = "gotowe.png";
        var splitterJob = new SplitterJob();
        var folderQfile = (folderPath, filename);
        string path = Path.Combine(folderPath, filename);
        var imageStream = File.OpenRead(path);
        DecoderOptions options = new();
        Image image = Image.Load(options, imageStream);
        
        // act
        string splitStrategyName = ISplitStrategyNames.FullPageStrategy;
        ISplitStrategy strategy = _strategyBase
            .GetNewStrategy(splitStrategyName);
        SplitInfo splitInfo = splitterJob.CreateSplitImages(
            folderQfile,
            strategy.HeightByWidth,
            strategy.Overlap,
            200);

        return splitInfo;
    }
}
