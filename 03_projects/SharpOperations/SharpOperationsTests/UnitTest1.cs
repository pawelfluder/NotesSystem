using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SharpOperationsProg.Operations.UniAddress;

namespace SharpOperationsTests;

using OutBorder01 = SharpFileServiceProg.AAPublic.OutBorder;
using OutBorder02 = SharpOperationsProg.AAPublic.OutBorder;

public class Tests
{
    private IFileService _file;
    private IOperationsService _operations;
    private IUniAddressOperations _uniAddress;

    [SetUp]
    public void Setup()
    {
        _file = OutBorder01.FileService();
        _operations = OutBorder02.OperationsService(_file);
        _uniAddress = _operations.UniAddress;
    }

    [Test]
    public void Test1()
    {
        string goupFolderPath = "/Users/pawelfluder/Dropbox/ebf8d4ba-06c2-43eb-a201-4d32d13656e4";
        string repoName = "Rama";
            
        // string goupFolderPath = "/Users/pawelfluder/Dropbox/0fc7da8d-3466-4964-a24c-dfc0d0fef87c";
        // string repoName = "Worldline";
        
        string repoFolderPath = goupFolderPath + "/" + repoName;
        int fileCount = Directory.EnumerateFiles(repoFolderPath, "*.*", SearchOption.AllDirectories).Count();
        int subDirCount = Directory.EnumerateDirectories(repoFolderPath, "*.*", SearchOption.AllDirectories).Count();
        int total = fileCount + subDirCount;
        
        List<string> addressesList = _uniAddress
            .GetAllAddressesInOneRepo(repoFolderPath);
        
    }
}
