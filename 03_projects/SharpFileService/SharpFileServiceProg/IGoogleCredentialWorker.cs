using System.Reflection;

namespace SharpConfigProg.APublic
{
    public interface IGoogleCredentialWorker
    {
        (string clientId, string clientSecret) GetCredentials(
            AssemblyName assemblyName,
            string embeddedResourceFile);
    }
}