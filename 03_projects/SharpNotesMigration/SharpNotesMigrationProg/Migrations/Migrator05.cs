using System;
using System.Collections.Generic;
using System.Linq;
using SharpFileServiceProg.AAPublic;
using SharpNotesMigrationProg.AAPublic;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SharpRepoServiceProg.AAPublic;
using SharpRepoServiceProg.AAPublic.Names;

namespace SharpNotesMigrationProg.Migrations;

internal class Migrator05 : IMigrator, IMigrator05
{
    private readonly IOperationsService _operationsService;
    private readonly IRepoService _repoService;
    private readonly IYamlOperations _yamlOperations;
    private bool agree;
    private readonly IFileService _fileService;

    public string Description
    {
        get
        {
            return @"Previously - there was 4 empty lines at the top of body file.
                         After update - there is no empty lines at the top.";
        }
    }

    // 
    public List<(int, string, string, string)> Changes { get; private set; }

    public Migrator05(
        IOperationsService operationsService,
        IRepoService repoService)
    {
        _operationsService = operationsService;
        _fileService = _operationsService.GetFileService();
        _repoService = repoService;
        _yamlOperations = _fileService.Yaml.Custom03;
        Changes = new List<(int, string, string, string)>();
    }

    public void MigrateEverything()
    {
        throw new NotImplementedException();
    }

    public void MigrateOneFolder((string Repo, string Loca) adrTuple)
    {
        var foundAddressList = _repoService.Methods
            .GetAllRepoAddresses(adrTuple).ToList();

        MigrateOneAddress(adrTuple);

        foreach (var foundAddress in foundAddressList)
        {
            MigrateOneAddress(foundAddress);
        }
    }

    private void MigrateOneAddress(
        (string Repo, string Loca) adrTuple)
    {
        var type = _repoService.Methods.GetType(adrTuple);

        if (type == UniType.Text.ToString())
        {
            var newText = RemoveTopEmptyLines(adrTuple);
            _repoService.Methods.PatchText(newText, adrTuple);
        }
    }

    private string RemoveTopEmptyLines((string Repo, string Loca) adrTuple)
    {
        var lines = _repoService.Methods.GetTextLines(adrTuple);
        var max = Math.Min(lines.Count, 4);
        for (var i = 0; i < max; i++)
        {
            var line = lines[0];
            if (line == string.Empty)
            {
                lines.RemoveAt(0);
            }

            if (line != string.Empty)
            {
                break;
            }
        }

        var text = string.Join("\n", lines);

        return text;
    }

    void IMigrator.MigrateOneAddress((string Repo, string Loca) address)
    {
        throw new NotImplementedException();
    }

    public void MigrateOneRepo(string repoName)
    {
        throw new NotImplementedException();
    }

    public void MigrateAllRepos()
    {
        throw new NotImplementedException();
    }

    public void SetAgree(bool agree)
    {
        this.agree = agree;
    }
}