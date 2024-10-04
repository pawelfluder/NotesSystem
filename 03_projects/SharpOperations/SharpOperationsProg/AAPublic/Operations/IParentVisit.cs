namespace SharpOperationsProg.AAPublic.Operations;

public interface IParentVisit
{
    public List<DirectoryInfo> Parents { get; }

    public void Visit(
        string path,
        Action<FileInfo> fileAction,
        Action<DirectoryInfo> directoryAction);
}