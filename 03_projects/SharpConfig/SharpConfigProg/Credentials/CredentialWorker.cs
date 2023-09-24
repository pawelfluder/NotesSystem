using Newtonsoft.Json.Linq;
using System.Reflection;

namespace SharpRepoBackendProg.Repetition
{
    internal class CredentialWorker
    {
        public (string clientId, string clientSecret) GetCredentials(string fileProjectPath)
        {
            var namespaceName = Assembly.GetCallingAssembly().GetName().Name;
            var result = new CredentialWorker().GetEmbeddedResource(namespaceName, fileProjectPath);

            JObject googleSearch = JObject.Parse(result);
            var clientId = googleSearch["installed"]["client_id"].ToString();
            var clientSecret = googleSearch["installed"]["client_secret"].ToString();

            return (clientId, clientSecret);
        }

        public string GetEmbeddedResource(string namespacename, string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = namespacename + "." + filename;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new Exception("CredentialWorker - Please check assembly file path, bacause file stream was null!");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }            
        }
    }
}
