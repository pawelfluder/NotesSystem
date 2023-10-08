namespace SharpFileServiceProg.Operations.Files
{
    public interface IPathsOperations
    {
        string MoveDirectoriesUp(string path, int level);
        string GetBinPath();
        string TryGetBinPath(out bool success);
        void CreateMissingDirectories(string path);
        string GetStartupProjectFolderPath();
        string GetProjectFolderPath(string projectName);
    }
}