using System.Runtime.InteropServices;
using SharpConfigProg.AAPublic;
using SharpConfigProg.Service;
using SharpOperationsProg.AAPublic.Operations;
using SharpSetup01Prog.Registrations;

namespace SharpSetup01Prog.Preparer;

internal class DefaultPreparer : IPreparer
{
    private readonly IGoogleCredentialWorker credentials;
    private IConfigService configService;
    private Dictionary<string, object> settingsDict;
    private readonly IOperationsService _operationsService;

    public DefaultPreparer(
        IOperationsService operationsService)
    {
        _operationsService = operationsService;
        credentials = operationsService.Credentials;
    }

    public Dictionary<string, object> Prepare()
    {
        DefaultRegistration reg = new();
        PrepareConfig();
        reg.SetTempDict(settingsDict);
        reg.Registrations();

        return settingsDict;
    }

    private void PrepareConfig()
    {
        settingsDict = new Dictionary<string, object>();
        
        string googleUserName = "notki.info@gmail.com";
        string googleApplicationName = "GameStatistics";
        string jsonFilePath = "public_google-cloud-secrets.json";
        
        string settingsFolderPath = _operationsService
            .Path.FindFolder(
                "01_settings",
                Directory.GetCurrentDirectory(),
                "3(2,2)");
        var repoRootPaths = GetRepoSearchPaths(settingsFolderPath);
        string googleCloudCredentialsPath =
            settingsFolderPath
            + "/"
            + jsonFilePath;
        bool exists = File.Exists(googleCloudCredentialsPath);
        if (!exists) { throw new FileNotFoundException("Google cloud credentials not found."); }

        string jsonFileContent = File.ReadAllText(googleCloudCredentialsPath);
        
        (string googleClientId, string googleClientSecret) = credentials
            .GetCredentials(jsonFileContent);

        settingsDict.Add(nameof(repoRootPaths), repoRootPaths);
        settingsDict.Add(nameof(googleClientId), googleClientId);
        settingsDict.Add(nameof(googleClientSecret), googleClientSecret);
        settingsDict.Add(nameof(googleUserName), googleUserName);
        settingsDict.Add(nameof(googleApplicationName), googleApplicationName);
    }

    public List<object> GetRepoSearchPaths(
        string settingsFolderPath)
    {
        string result = settingsFolderPath + "/" + 
        return repoSearchPaths;
    }

    public void TryGetMacPath(ref string path)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            path = "/Users/pawelfluder/Dropbox";
        }
    }

    public void TryGetWindowsPath(ref string path)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            //path = "D:/03_synch/05_backup/23-10-08_NotesSystem";
            path = "C:/03_synch/Dropbox";
        }
    }

    public bool GetGoogleCredentials(
        out string clientId,
        out string clientSecret)
    {
        var s1 = configService.TryGetSettingAsString("googleClientId", out clientId);
        var s2 = configService.TryGetSettingAsString("googleClientSecret", out clientSecret);
        return s1 && s2;
    }

    public void SetConfigService(IConfigService configService)
    {
        this.configService = configService;
    }
}
