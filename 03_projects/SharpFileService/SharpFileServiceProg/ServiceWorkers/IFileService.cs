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
            string GetAddressString((string, string) adrTuple);
            (string, string) SelectAddress((string Repo, string Loca) address, int index);
            string IndexToString(int? index);
            int StringToIndex(string input);
            bool TryStringToIndex(string input, out int index);
            string LastTwoChar(string input);
            bool IsCorrectIndex(string input);
            bool IsCorrectIndex(string input, out int index);
            int GetLocaLast(string loca);
            (string, string) JoinIndexWithLoca((string Repo, string Loca) adrTuple, int? index);

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
