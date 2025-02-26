using System;
using System.Collections.Generic;
using System.Linq;
using SharpNotesMigrationProg.AAPublic;
using SharpRepoServiceProg.AAPublic;
using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpNotesMigrationProg.Migrations;

internal class Migrator04 : IMigrator, IMigrator04
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
            return @"Previously - there was no ""id"" and ""type"" in config.
                         After update - those two values are added to config file.";
        }
    }

    public List<(int, string, string, string)> Changes { get; private set; }

    public Migrator04(
        IOperationsService operationsService,
        IRepoService repoService)
    {
        _operationsService = operationsService;
        _repoService = repoService;
        _fileService = _operationsService.GetFileService();
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

        //MigrateOneAddress(address);
        foreach (var foundAddress in foundAddressList)
        {
            MigrateOneAddress(foundAddress);
        }
    }

    public void MigrateOneAddress((string Repo, string Loca) adrTuple)
    {
        var dict = _repoService.Methods.GetConfigKeyDict(adrTuple);
        var type = _repoService.Methods.GetType(adrTuple);
        var s1 = dict.TryAdd("id", Guid.NewGuid().ToString());
        var s2 = dict.TryAdd("type", type);

        if (agree)
        {
            _repoService.Methods.CreateConfig(adrTuple, dict);
        }
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

    //void IMigrator.MigrateOneAddress((string Repo, string Loca) adrTuple)
    //{
    //    throw new NotImplementedException();
    //}
}