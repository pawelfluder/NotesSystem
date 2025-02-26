using System.ComponentModel;
using SharpConfigProg.AAPublic;
using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SharpSetup01Prog.Registrations;
using OutBorder01 = SharpFileServiceProg.AAPublic.OutBorder;
using OutBorder02 = SharpOperationsProg.AAPublic.OutBorder;

namespace SharpSetup01Prog.Preparer;
internal class DefaultPreparer : IPreparer
{
    private IGoogleCredentialWorker _credentials;
    private Dictionary<string, object> _settingsDict;
    private IOperationsService _operationsService;
    private IConfigService _configService;
    private bool _isPreparationDone;
    private IFileService _fileService;
    private IContainer _container;

    public Dictionary<string, object> Prepare()
    {
        if (_isPreparationDone)
        {
            return _settingsDict;
        }
        
        _fileService = OutBorder01.FileService();
        _operationsService = OutBorder02.OperationsService(_fileService);
        _credentials = _operationsService.Credentials;
        
        PrepareSettings();
        DefaultRegistration reg = new();
        reg.SettingsDict = _settingsDict;
        reg.FileService = _fileService;
        reg.OperationsService = _operationsService;
        reg.Registrations();

        _isPreparationDone = true;
        return _settingsDict;
    }

    private void PrepareSettings()
    {
        // currentPath
        string currentPath = Directory.GetCurrentDirectory();
        Console.WriteLine("currentPath: " + currentPath);
        
        // currentPath2
        string currentPath2 = Environment.CurrentDirectory;
        Console.WriteLine("currentPath2: " + currentPath2);
        
        
        // group paths, settings folder, database folder
        string settingsFolderPath = _operationsService
            .Path.FindFolder(
                "02_settings",
                currentPath,
                "0(3,3)"); // 5(0,1) 5(2,3)
        string databaseFolderPath = _operationsService
            .Path.FindFolder(
                "01_database",
                settingsFolderPath,
                "1(0,0)");
        List<object> repoRootPaths = new()
        {
            databaseFolderPath
        };
        
        // google cloud
        string googleUserName = "abcdefgh@gmail.com";
        string googleApplicationName = "ApplicationName";
        
        string jsonFilePath = "public_google-cloud-secrets.json";
        string googleCloudCredentialsPath =
            settingsFolderPath
            + "/"
            + jsonFilePath;
        bool exists = File.Exists(googleCloudCredentialsPath);
        if (!exists) { throw new FileNotFoundException("Google cloud credentials not found."); }
        string jsonFileContent = File.ReadAllText(googleCloudCredentialsPath);
        (string googleClientId, string googleClientSecret) = _credentials
            .GetCredentials(jsonFileContent);
        
        _settingsDict= new();
        // settings for google cloud
        _settingsDict.Add(nameof(repoRootPaths), repoRootPaths);
        _settingsDict.Add(nameof(settingsFolderPath), settingsFolderPath);
        _settingsDict.Add(nameof(databaseFolderPath), databaseFolderPath);
        // settings for group paths, settings folder, database folder
        _settingsDict.Add(nameof(googleClientId), googleClientId);
        _settingsDict.Add(nameof(googleClientSecret), googleClientSecret);
        _settingsDict.Add(nameof(googleUserName), googleUserName);
        _settingsDict.Add(nameof(googleApplicationName), googleApplicationName);
    }

    public List<object> GetRepoSearchPaths(
        string settingsFolderPath)
    {
        return new List<object> { settingsFolderPath };
    }

    public void SetConfigService(
        IConfigService configService)
    {
        _configService = configService;
    }
    
    public void SetContainer(
        IContainer container)
    {
        _container = container;
    }
}
