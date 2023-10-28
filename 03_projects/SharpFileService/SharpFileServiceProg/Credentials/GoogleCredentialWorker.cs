using Newtonsoft.Json.Linq;
using SharpConfigProg.AAPublic;
using System.Reflection;

namespace SharpConfigProg.Credentials
{
    internal class GoogleCredentialWorker : IGoogleCredentialWorker
    {
        public (string clientId, string clientSecret) GetCredentials(
            AssemblyName assemblyName,
            string embeddedResourceFile)
        {
            var result = GetEmbeddedResource(assemblyName, embeddedResourceFile);

            JObject googleSearch = JObject.Parse(result);
            var clientId = googleSearch["installed"]["client_id"].ToString();
            var clientSecret = googleSearch["installed"]["client_secret"].ToString();

            return (clientId, clientSecret);
        }

        public string GetEmbeddedResource(AssemblyName assemblyName, string filename)
        {
            var namespacename = assemblyName.Name;
            var resourceName = namespacename + "." + filename;
            var assembly = Assembly.Load(assemblyName);

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
