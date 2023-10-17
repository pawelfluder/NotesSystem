using GoogleDocsServiceProj.Service;

namespace SharpGoogleDocsProg.Repetition
{
    public class OutBorder
    {
        public static GoogleDocsService GoogleDocsService()
        {
            //var credentialWorker = new CredentialWorker();
            //var credentials = credentialWorker.GetCredentials();
            var aplicationName = "";
            var scopes = new List<string>();

            var googleDocsService = new GoogleDocsService();

            //var googleDocsService = new GoogleDocsService(
            //    credentials.clientId,
            //    credentials.clientSecret,
            //    aplicationName,
            //    scopes);
            return googleDocsService;
        }
    }
}
