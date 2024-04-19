using Newtonsoft.Json.Linq;
using System.Reflection;

namespace TinderImport.Repetition
{
    internal class CredentialWorker
    {
        public (string clientId, string clientSecret) GetCredentials()
        {
            var namespaceName = Assembly.GetCallingAssembly().GetName().Name; // "GoogleDocsServiceProj";
            var fileName = "Repetition.EmbeddedResources.21-09-30_Notki-info_GameStatistics.json";
            string[] Scopes = { };
            string applicationName = "GoogleDriveService";
            var result = new CredentialWorker().GetEmbeddedResource(namespaceName, fileName);

            JObject googleSearch = JObject.Parse(result);
            IList<JToken> results = googleSearch["installed"].Children().ToList();
            var clientId = googleSearch["installed"]["client_id"].ToString();
            var clientSecret = googleSearch["installed"]["client_secret"].ToString();

            return (clientId, clientSecret);
        }

        public string GetEmbeddedResource(string namespacename, string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = namespacename + "." + filename;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }
    }
}
