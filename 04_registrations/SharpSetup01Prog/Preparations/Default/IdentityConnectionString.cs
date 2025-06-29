using SharpIdentityProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SharpOperationsProg.Operations.Path;

namespace SharpSetup01Prog.Preparations.Default;

public class IdentityDbConnectionString : IIdentityDbConnectionString
{
    private readonly IFolderFinder _folderFinderOp;
    private readonly string _searchExpression;
    private string? _dbFolderPath;

    public IdentityDbConnectionString(
        string dbSearchExpression)
    {
        _searchExpression  = dbSearchExpression;
        _folderFinderOp = IBackendOperations.FolderFinder;
    }

    public string GetConnStr()
    {
        string dbFilePath = GetDbFilePath();
        string connectionString = "Data Source=" + dbFilePath;
        return connectionString;
    }

    public string GetDbFilePath()
    {
        string dbFilePath = GetDbFolderPath() + "/" + GetDbFileName();
        return dbFilePath;
    }

    public string GetDbFolderPath()
    {
        if (_dbFolderPath != null)
            return _dbFolderPath;
        
        string folderName = "IdentityDatabase";
        _dbFolderPath = _folderFinderOp.FindFolder(
            folderName, 
            Environment.CurrentDirectory, _searchExpression,
            GetType());
        _dbFolderPath = _dbFolderPath.Replace('\\', '/');
        return _dbFolderPath;
    }

    public string GetDbFileName() => "IdentityDatabase.db";
}
