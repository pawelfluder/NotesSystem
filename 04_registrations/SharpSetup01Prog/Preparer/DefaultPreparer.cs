using SharpConfigProg.AAPublic;
using SharpFileServiceProg.AAPublic;
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

    public Dictionary<string, object> Prepare()
    {
        _fileService = OutBorder01.FileService();
        _operationsService = OutBorder02.OperationsService(_fileService);
        _credentials = _operationsService.Credentials;
        
        if (_isPreparationDone)
        {
            return _settingsDict;
        }
        
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
        _settingsDict= new();
        string googleUserName = "abcdefgh@gmail.com";
        string googleApplicationName = "ApplicationName";
        string jsonFilePath = "public_google-cloud-secrets.json";
        
        string settingsFolderPath = _operationsService
            .Path.FindFolder(
                "02_settings",
                Directory.GetCurrentDirectory(),
                "3(2,2)");
        string databaseFolderPath = _operationsService
            .Path.FindFolder(
                "01_database",
                settingsFolderPath,
                "1(0,0)");
        List<object> repoRootPaths = new()
        {
            databaseFolderPath
        };
        string googleCloudCredentialsPath =
            settingsFolderPath
            + "/"
            + jsonFilePath;
        bool exists = File.Exists(googleCloudCredentialsPath);
        if (!exists) { throw new FileNotFoundException("Google cloud credentials not found."); }

        string jsonFileContent = File.ReadAllText(googleCloudCredentialsPath);
        
        (string googleClientId, string googleClientSecret) = _credentials
            .GetCredentials(jsonFileContent);

        _settingsDict.Add(nameof(settingsFolderPath), settingsFolderPath);
        _settingsDict.Add(nameof(databaseFolderPath), databaseFolderPath);
        _settingsDict.Add(nameof(repoRootPaths), repoRootPaths);
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
}
