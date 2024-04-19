namespace SharpFileServiceProg.Operations.Files
{
    public interface IPathsOperations
    {
        string FindFolder(string searchFolderName, string inputFolderPath, string expression);
        string MoveDirectoriesUp(string path, int level);
        string GetBinPath();
        string TryGetBinPath(out bool success);
        void CreateMissingDirectories(string path);
        string GetStartupProjectFolderPath();
        string GetProjectFolderPath(string projectName);
    }
}