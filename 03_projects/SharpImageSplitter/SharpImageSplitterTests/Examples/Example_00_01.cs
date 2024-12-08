using SharpImageSplitterProg.AAPublic.Names;
using SharpImageSplitterProg.AAPublic.Strategies;
using SharpImageSplitterProg.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace SharpImageSplitterTests.Examples;

internal partial class ExamplesProvider
{
    public SplitInfo Example_00_01()
    {
        // arrange
        string folderPath = @"/Volumes/qnap/01_files_refrenced/2024/03_files_photos/13_scam_krypto/05_Zespół-Binance-analizuje-formularz-zgłoszeniowy-oraz-potwierdzenie-oszustwo";
        string filename = "Binance_team.PNG";
        string path = Path.Combine(folderPath, filename);
        var imageStream = File.OpenRead(path);
        DecoderOptions options = new();
        Image image = Image.Load(options, imageStream);
        
        // act
        string splitStrategyName = ISplitStrategyNames.Winder2Strategy;
        ISplitStrategy strategy = _strategyBase
            .GetNewStrategy(splitStrategyName);
        SplitInfo splitInfo = _recalculate.Do(
            strategy.HeightByWidth,
            strategy.Overlap,
            image.Width,
            image.Height,
            5000);
        
        return splitInfo;
    }
}
