using GoogleDocsServiceProj.Service;
using SharpGoogleDriveProg.Service;
using TinderImport.Repetition;

namespace SharpNotesExporterTests.Repetition
{
    internal class Border
    {
        public static GoogleDocsService NewGoogleDocsService()
        {
            var credentialWorker = new CredentialWorker();
            var credentials = credentialWorker.GetCredentials();
            var aplicationName = "";
            var scopes = new List<string>();
            var googleDocsService = new GoogleDocsService(
                credentials.clientId,
                credentials.clientSecret,
                aplicationName,
                scopes);
            return googleDocsService;
        }

        public static GoogleDriveService NewGoogleDriveService()
        {
            var credentialWorker = new CredentialWorker();
            var credentials = credentialWorker.GetCredentials();
            var scopes = new List<string>();
            var googleDocsService = new GoogleDriveService(
                credentials.clientId,
                credentials.clientSecret);
            return googleDocsService;
        }
    }
}
