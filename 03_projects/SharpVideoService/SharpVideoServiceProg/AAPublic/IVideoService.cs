namespace SharpVideoServiceProg.AAPublic
{
    public interface IVideoService
    {
        Task<bool> PosterWithAudio(string imageFilePath, string audioFilePath, string outputFilePath);
    }
}