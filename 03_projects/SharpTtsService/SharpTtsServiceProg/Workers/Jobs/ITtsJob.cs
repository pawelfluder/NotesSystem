using System.Globalization;

namespace SharpTtsServiceProg.Workers.Jobs;

public interface ITtsJob
{
    void SetVoiceSettings2(CultureInfo culture);
    Task Pause();
    Task Resume();
    Task PlStartNew(
        object builder);
    Task EnStartNew(
        object builder);
    Task Stop();
    Task SaveAudioFile(
        string folderPath,
        string fileName,
        object builder);

    object GetBuilder(
        (string, string) adrTuple,
        CultureInfo culture);
}
