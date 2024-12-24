using System.Globalization;
using SharpOperationsProg.AAPublic.Operations;
using SharpRepoServiceProg.AAPublic;
using SharpRepoServiceProg.AAPublic.Names;
using SharpTtsServiceProg.Workers.Jobs;
using SharpVideoServiceProg.AAPublic;

namespace SharpTtsServiceProg.Workers.Fasades;

public class RepoTtsWorker : RunMethodsBase
{
    private readonly IRepoService _repoService;
    private readonly IVideoService _videoService;
    private readonly ITtsJob _ttsJob;

    public RepoTtsWorker(
        IRepoService repoService,
        IVideoService videoService,
        ITtsJob ttsJob)
    {
        _repoService = repoService;
        _videoService = videoService;
        _ttsJob = ttsJob;
    }

    public async Task Stop()
    {
        await _ttsJob.Stop();
    }

    public async Task PlStartNew(
        object builder)
    {
        await _ttsJob.PlStartNew(builder);
    }
    
    public async Task EnStartNew(
        string repo,
        string loca)
    {
        (string Repo, string Loca) adrTuple = (repo, loca);
        string? textFilePath = _repoService.Methods
            .GetBodyPath(adrTuple);
        await _ttsJob.EnStartNew(textFilePath);
    }
    
    public async Task Pause()
    {
        await _ttsJob.Pause();
    }
    
    public async Task EnSaveAudio(
        string repo, string loca)
    {
        (string Repo, string Loca) adrTuple = (repo, loca);
        string? textFilePath = _repoService.Methods
            .GetBodyPath(adrTuple);
        CultureInfo culture = new("en-GB");
        object builder = _ttsJob.GetBuilder(adrTuple, culture);
        string fileName = UniFileNames.BodyTxt;
        await _ttsJob.SaveAudioFile(textFilePath, fileName, builder);
    }

    // public async Task VoiceToVideo(
    //     (string Repo, string Loca) adrTuple)
    // {
    //     var folderPath = _repoService.Methods.GetElemPath(adrTuple);
    //     //var folderPath = await PlSaveAudio(adrTuple);
    //     var audioFilePath = folderPath + "/" + fileName + ".wav";
    //     var imageFilePath = "Output/background.png";
    //     var videoFilePath = folderPath + "/" + fileName + ".mp4";
    //     await _videoService.PosterWithAudio(imageFilePath, audioFilePath, videoFilePath);
    // }

    public async Task Resume()
    {
        await _ttsJob.Resume();
    }

    private string GetText((string Repo, string Loca) adrTuple)
    {
        string? text = _repoService.Methods.GetText2(adrTuple);
        text = text.Replace("//", "");
        return text;
    }
}