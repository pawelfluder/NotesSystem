using System.Reflection;
using Newtonsoft.Json.Linq;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpOperationsProg.Operations.Credentials;

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

    public AssemblyName GetAssemblyName(object obj)
    {
        var assembly = Assembly.GetAssembly(obj.GetType());
        var assebmlyName = assembly.GetName();
        return assebmlyName;
    }

    public string GetEmbeddedResource(AssemblyName assemblyName, string filename)
    {
        string? namespacename = assemblyName.Name;
        string resourceName = namespacename + "." + filename;
        Assembly assembly = Assembly.Load(assemblyName);

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
            {
                throw new Exception("CredentialWorker - Please check assembly file path, because file stream was null!");
            }

            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }
    }

    public Stream GetEmbeddedResourceStream(
        AssemblyName assemblyName,
        string filename)
    {
        string? namespacename = assemblyName.Name;
        string resourceName = namespacename + "." + filename;
        Assembly assembly = Assembly.Load(assemblyName);

        // correct example - SharpGoogleDriveProg.EmbeddedResources.Template05.docx
        Stream? stream = assembly.GetManifestResourceStream(resourceName);

        if (stream == null)
        {
            throw new Exception("CredentialWorker - Please check assembly file path, bacause file stream was null!");
        }

        StreamReader reader = new(stream);
        return stream;
    }
}