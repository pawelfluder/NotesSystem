﻿using System.Reflection;

namespace SharpConfigProg.AAPublic
{
    public interface IGoogleCredentialWorker
    {
        (string clientId, string clientSecret) GetCredentials(
            AssemblyName assemblyName,
            string embeddedResourceFile);

        string GetEmbeddedResource(AssemblyName assemblyName, string filename);
        Stream GetEmbeddedResourceStream(AssemblyName assemblyName, string filename);
    }
}