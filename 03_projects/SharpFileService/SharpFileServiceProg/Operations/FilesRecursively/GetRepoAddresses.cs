using SharpFileServiceProg.Service;

namespace SharpRepoServiceProg.FileOperations
{
    public class GetRepoAddresses : IRepoAddressesObtainer
    {
        private readonly IFileService fileService;
        private List<(string repo, string loca)> addressesList;
        private string repoName;
        private IFileService.IParentVisit vdr;

        public GetRepoAddresses(IFileService fileService)
        {
            this.fileService = fileService;
            ReInitialize();
        }

        private void ReInitialize()
        {
            addressesList = new List<(string repo, string loca)>();
        }

        public List<(string, string)> Visit(string path)
        {
            this.repoName = Path.GetFileName(path);
            vdr = fileService.File.GetNewVisitDirectoriesRecursivelyWithParentMemory();
            var fileAction = FileAction;
            var folderAction = FolderAction;
            vdr.Visit(path, fileAction, folderAction);
            var result = new List<(string repo, string loca)>(addressesList);
            ReInitialize();
            return result;
        }

        private void FileAction(FileInfo fileInfo)
        {
        }

        private void FolderAction(DirectoryInfo directoryInfo)
        {
            if (fileService.Index
                .IsCorrectIndex(directoryInfo.FullName, out var index))
            {
                var parents = vdr.Parents;
                var names = parents.Select(x => x.Name);
                var loca = string.Join('/', names);
                var address = (repoName, loca);
                addressesList.Add(address);
            }
        }
    }
}
