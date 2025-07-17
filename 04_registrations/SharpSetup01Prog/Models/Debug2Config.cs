using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpSetup01Prog.Models;

public class Debug2Config : ConfigBase
{
    public Debug2Config(
        IOperationsService operations,
        IGoogleCredentialWorker googleCredentials)
    {
        // current path
        currentDirectoryPath = Directory.GetCurrentDirectory();
        
        // repoSearchPath01
        string expression01 = "0(1,2)";
        repoSearchPath01 = IBackendOperations.FolderFinder
            .FindFolder(
                "01_database",
                currentDirectoryPath,
                expression01,
                GetType());
        
        // repoSearchPath02
        string dropboxPath = "/Users/pawelfluder/Dropbox";
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
            repoSearchPath01,
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
