using SharpoperationsServiceProg.AAPublic;
using SharpOperationsProg.AAPublic;

namespace SharpOperationsProg.Operations.FilesRecursively
{
    public class GetRepoAddresses : IRepoAddressesObtainer
    {
        private readonly IOperationsService operationsService;
        private List<string> locaList;
        private string repoName;
        private IParentVisit vdr;

        public GetRepoAddresses(IOperationsService operationsService)
        {
            this.operationsService = operationsService;
            ReInitialize();
        }

        private void ReInitialize()
        {
            locaList = new List<string>();
        }

        public List<string> Visit(string path)
        {
            this.repoName = System.IO.Path.GetFileName(path);
            vdr = operationsService.File.GetNewVisitDirectoriesRecursivelyWithParentMemory();
            var fileAction = FileAction;
            var folderAction = FolderAction;
            vdr.Visit(path, fileAction, folderAction);
            var result = new List<string>(locaList);
            ReInitialize();
            return result;
        }

        private void FileAction(FileInfo fileInfo)
        {
        }

        private void FolderAction(DirectoryInfo directoryInfo)
        {
            if (operationsService.Index
                .IsCorrectIndex(directoryInfo.FullName, out var index))
            {
                var parents = vdr.Parents;
                var names = parents.Select(x => x.Name);
                var loca = string.Join('/', names);
                locaList.Add(loca);
            }
        }
    }
}
