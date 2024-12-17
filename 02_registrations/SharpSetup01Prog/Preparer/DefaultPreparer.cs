using System.Runtime.InteropServices;
using SharpConfigProg.AAPublic;
using SharpConfigProg.Service;
using SharpOperationsProg.AAPublic.Operations;
using SharpSetup01Prog.Registrations;

namespace SharpSetup01Prog.Preparer;

internal class DefaultPreparer : IPreparer
{
    private readonly IGoogleCredentialWorker credentials;
    private readonly Dictionary<string, object> settingsDict = new Dictionary<string, object>();
    private readonly IOperationsService _operationsService;
    private IConfigService configService;

    public DefaultPreparer(
        IOperationsService operationsService)
    {
        _operationsService = operationsService;
        credentials = operationsService.Credentials;
    }

    public Dictionary<string, object> Prepare()
    {
        PrepareSettings();
        DefaultRegistration reg = new();
        reg.SetSettingsDict(settingsDict);
        reg.Registrations();

        return settingsDict;
    }

    private void PrepareSettings()
    {
        string googleUserName = "notki.info@gmail.com";
        string googleApplicationName = "GameStatistics";
        string jsonFilePath = "public_google-cloud-secrets.json";
        
        string settingsFolderPath = _operationsService
            .Path.FindFolder(
                "01_settings",
                Directory.GetCurrentDirectory(),
                "3(2,2)");
        List<object> repoRootPaths = new()
        {
            settingsFolderPath
        };
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
        return new List<object> { settingsFolderPath };
    }

    public bool GetGoogleCredentials(
        out string clientId,
        out string clientSecret)
    {
        bool s1 = configService.TryGetSettingAsString("googleClientId", out clientId);
        bool s2 = configService.TryGetSettingAsString("googleClientSecret", out clientSecret);
        return s1 && s2;
    }

    public void SetConfigService(IConfigService configService)
    {
        this.configService = configService;
    }
}
