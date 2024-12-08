namespace SharpImageSplitterProg.AAPublic;

public interface ISplitterJob
{
    void CreateSplitImages(
        (string folderPath, string fileName) folderQfile,
        string splitStrategyName);
}