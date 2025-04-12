namespace SharpSetup01Prog.Models;

public abstract class ConfigBase
{
    // repos
    protected string currentDirectoryPath;
    protected List<string> repoRootPaths;
    protected string settingsFolderPath;
    protected string repoSearchPath01;
    protected string repoSearchPath02;
    protected string repoSearchPath03;
    
    // google cloud
    protected string googleClientId;
    protected string googleClientSecret;
    protected string googleUserName;
    protected string googleApplicationName;

    public void AddRangeForSettings(
        Dictionary<string, object> settingsDict)
    {
        // repos
        settingsDict.Add(nameof(repoRootPaths), repoRootPaths);
        settingsDict.Add(nameof(settingsFolderPath), settingsFolderPath);
        settingsDict.Add(nameof(repoSearchPath01), repoSearchPath01);
        
        // google cloud
        settingsDict.Add(nameof(googleClientId), googleClientId);
        settingsDict.Add(nameof(googleClientSecret), googleClientSecret);
        settingsDict.Add(nameof(googleUserName), googleUserName);
        settingsDict.Add(nameof(googleApplicationName), googleApplicationName);
    }
}
