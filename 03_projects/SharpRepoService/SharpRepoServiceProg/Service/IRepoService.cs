using SharpRepoServiceProg.RepoOperations;

namespace SharpRepoServiceProg.Service
{
    public interface IRepoService
    {
        RepoServiceMethods Methods { get; }

        public enum ConfigKeys
        {
            googleDocId,
            name,
        }
    }
}