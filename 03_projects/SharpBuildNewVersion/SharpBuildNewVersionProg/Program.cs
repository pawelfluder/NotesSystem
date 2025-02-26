using Microsoft.Build.Locator;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Logging;

class Program
{
    static void Main()
    {
        MSBuildLocator.RegisterDefaults(); // Rejestracja MSBuild

        string projectPath = @"C:\ścieżka\do\projektu.csproj";

        var projectCollection = new ProjectCollection();
        var buildParameters = new BuildParameters(projectCollection)
        {
            Loggers = new[] { new ConsoleLogger() }
        };

        // 3. Konfiguracja publikacji w trybie Release do D:/output
        var globalProperties = new Dictionary<string, string>
        {
            { "Configuration", "Release" },
            { "PublishDir", outputPath },
            { "DeployOnBuild", "true" },
            { "PublishReadyToRun", "true" }
        };

        var buildRequest = new BuildRequestData(projectPath, globalProperties, null, new[] { "Build", "Publish" }, null);
        var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest);

        Console.WriteLine($"Wynik kompilacji: {buildResult.OverallResult}");
        Console.WriteLine($"Wynik kompilacji: {buildResult.OverallResult}");
    }
}
