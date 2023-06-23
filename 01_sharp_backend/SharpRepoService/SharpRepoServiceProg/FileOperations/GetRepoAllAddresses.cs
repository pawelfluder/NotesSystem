using SharpFileServiceProg.Service;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpRepoServiceProg.FileOperations
{
    internal class GetRepoAllAddresses
    {
        private readonly FileService fileService;
        private List<(string repo, string loca)> addressesList;
        private string repoName;
        private FileService.IParentVisit vdr;

        public GetRepoAllAddresses(FileService fileService)
        {
            this.fileService = fileService;
            ReInitialize();
        }

        private void ReInitialize()
        {
            addressesList = new List<(string repo, string loca)>();
        }

        public List<(string repo, string loca)> Visit(string path)
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
