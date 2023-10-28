using System.Reflection;

namespace SharpConfigProg.AAPublic
{
    public interface IGoogleCredentialWorker
    {
        (string clientId, string clientSecret) GetCredentials(
            AssemblyName assemblyName,
            string embeddedResourceFile);
    }
}