using SharpRepoServiceProg.FileOperations;

namespace SharpFileServiceProg.Service
{
    public partial interface IFileService
    {
        public interface IFileWrk
        {
            IVisit GetNewRecursivelyVisitDirectory();
            IParentVisit GetNewVisitDirectoriesRecursivelyWithParentMemory();
            IRepoAddressesObtainer NewRepoAddressesObtainer();
        }

        public interface IIndexWrk
        {
            string IndexToString(int index);
            int StringToIndex(string input);
            int TryStringToIndex(string input);
            string LastTwoChar(string input);
            bool IsCorrectIndex(string input);
            bool IsCorrectIndex(string input, out int index);
            int GetLocaLast(string loca);
        }

        public interface IYamlWrk
        {
            IYamlOperations Dotnet { get; }
            IYamlOperations Sharp { get; }
            IYamlOperations Byjson { get; }
            IYamlOperations Custom01 { get; }
            IYamlOperations Custom02 { get; }
            IYamlOperations Custom03 { get; }
        }
    }
}
