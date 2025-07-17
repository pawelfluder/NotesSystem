using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpSetup01Prog.Models;

public class ReleaseLocalConfig : ConfigBase
{
    public ReleaseLocalConfig(
        IOperationsService operations,
        IGoogleCredentialWorker googleCredentials)
    {
        // repoSearchPath02
        string dropboxPath2 = "/Users/pawelfluder/Dropbox";
        string dropboxPath = "C:/03_Synch/Dropbox";
        repoSearchPath02 = dropboxPath;
        
        // settings path
        string expression02 = "0(0,0)";
        settingsFolderPath = IBackendOperations.FolderFinder
            .FindFolder(
                "02_settings",
                repoSearchPath02,
                expression02,
                GetType());
        
        // root paths
        repoRootPaths = new List<string>
        {
            repoSearchPath02,
        };
        
        // google cloud
        googleUserName = "notki.info@gmail.com";
        googleApplicationName = "GameStatistics";
        string jsonFilePath = "google-cloud-secrets.json";
        string googleCloudCredentialsPath =
            settingsFolderPath
            + "/"
            + jsonFilePath;
        bool exists = File.Exists(googleCloudCredentialsPath);
        if (!exists) { throw new FileNotFoundException("Google cloud credentials not found."); }
        string jsonFileContent = File.ReadAllText(googleCloudCredentialsPath);
        (googleClientId, googleClientSecret) = googleCredentials
            .GetCredentials(jsonFileContent);
    }
}