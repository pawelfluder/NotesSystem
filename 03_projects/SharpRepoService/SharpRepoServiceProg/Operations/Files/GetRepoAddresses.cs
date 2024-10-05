﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpFileServiceProg.AAPublic;

namespace SharpRepoServiceProg.Operations.Files;

internal class GetRepoAddresses : IRepoAddressesObtainer
{
    private readonly OperationsService _operationsService;
    private readonly IFileService _fileService;
    private List<string> locaList;
    private IParentVisit vdr;
    private string _repoName;

    public GetRepoAddresses(
        IFileService fileService,
        OperationsService operationsService)
    {
        _fileService = fileService;
        _operationsService = operationsService;
        ReInitialize();
    }

    private void ReInitialize()
    {
        locaList = new List<string>();
    }

    public List<string> Visit(string path)
    {
        _repoName = System.IO.Path.GetFileName(path);
        vdr = _fileService.File.GetNewVisitDirectoriesRecursivelyWithParentMemory();
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
        if (_operationsService.Index
            .IsCorrectIndex(directoryInfo.FullName, out var index))
        {
            var parents = vdr.Parents;
            var names = parents.Select(x => x.Name);
            var loca = string.Join('/', names);
            locaList.Add(loca);
        }
    }
}