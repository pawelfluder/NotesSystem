using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpSetup01Prog.Models;

public class DebugConfig : ConfigBase
{
    public DebugConfig(
        IOperationsService operations,
        IGoogleCredentialWorker googleCredentials)
    {
        // current path
        currentDirectoryPath = Directory.GetCurrentDirectory();
        
        // repoSearchPath01
        string expression01 = "5(2,3)";
        repoSearchPath01 = operations
            .Path.FindFolder(
                "01_database",
                currentDirectoryPath,
                expression01);
        
        // settings path
        string expression02 = "1(0,0)";
        settingsFolderPath = operations
            .Path.FindFolder(
                "02_settings",
                repoSearchPath01,
                expression02);
        
        // root paths
        repoRootPaths = new List<string>
        {
            repoSearchPath01
        };
        
        // google cloud
        string googleUserName = "abcdefgh@gmail.com";
        string googleApplicationName = "ApplicationName";
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
