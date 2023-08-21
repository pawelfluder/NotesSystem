using SharpRepoServiceProg.RepoOperations;
using System.Collections.Generic;

namespace SharpRepoServiceProg.Service
{
    public interface IRepoService
    {
        RepoServiceMethods Methods { get; }

        void Initialize(List<string> searchPaths);

        public enum ConfigKeys
        {
            googleDocId,
            name,
        }
    }
}