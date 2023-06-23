using SharpFileServiceProg.Operations.Index;

namespace SharpFileServiceProg.Service
{
    public partial class FileService : IFileService
    {
        public IFileWrk File { get; private set; }
        public IIndexWrk Index { get; private set; }
        public IYamlWrk Yaml { get; private set; }

        public FileService()
        {
            File = new FileWrk();
            Index = new IndexOperations();
            Yaml = new YamlWorker();
        }
    }

    public partial class FileService
    {
        public interface IFileWrk
        {
            IVisit GetNewRecursivelyVisitDirectory();
            IParentVisit GetNewVisitDirectoriesRecursivelyWithParentMemory();
        }

        public interface IIndexWrk
        {
            string IndexToString(int index);
            int StringToIndex(string input);
            int TryStringToIndex(string input);
            string LastTwoChar(string input);
            bool IsCorrectIndex(string input);
            bool IsCorrectIndex(string input, out int index);
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
