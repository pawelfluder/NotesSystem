using SharpIdentityProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SharpOperationsProg.Operations.Path;

namespace SharpSetup01Prog.Preparations.Default;

public class IdentityDbConnectionProvider : IIdentityDbConnectionProvider
{
    private readonly IFolderFinder _folderFinderOp;
    private string _dbFolderPath;
    public IdentityDbConnectionProvider()
    {
        _dbFolderPath = string.Empty;
        _folderFinderOp = IBackendOperations.FolderFinder;
        GetDbFolderPath();
    }

    public string GetConnStr()
    {
        string connectionString = "Data Source=" + GetDbFilePath();
        return connectionString;
    }

    public string GetDbFilePath()
    {
        string dbFilePath = _dbFolderPath + "/" + DbFileName;
        return dbFilePath;
    }

    public string GetDbFolderPath()
    {
        if (!string.IsNullOrEmpty(_dbFolderPath))
            return _dbFolderPath;
        
        string folderName01 = "01_database";
        string path01 = _folderFinderOp.FindFolder(
            folderName01, 
            Environment.CurrentDirectory,
            "2(0,1)",
            GetType());
        
        string folderName02 = "IdentityDatabase";
        _dbFolderPath = _folderFinderOp.FindFolder(
            folderName02,
            path01,
            "0(0,1)",
            GetType());
        _dbFolderPath = _dbFolderPath.Replace('\\', '/');
        return _dbFolderPath;
    }

    public string DbFileName => "IdentityDatabase.db";
}
