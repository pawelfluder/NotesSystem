﻿using System;
using System.Collections.Generic;
using SharpNotesMigrationProg.AAPublic;
using SharpNotesMigrationProg.Service;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SharpRepoServiceProg.AAPublic;

namespace SharpNotesMigrationProg.Migrations;

internal class Migrator01 : IMigrator, IMigrator01
{
    private readonly IOperationsService operationsService;
    private readonly IRepoService repoService;

    public string Description
    {
        get
        {
            return @"Previously - first line was a name value.
                         After update - name value is a part of yaml.
                         And is written like this: ""name"": ""my super name""";
        }
    }

    public Migrator01(
        IOperationsService operationsService,
        IRepoService repoService)
    {
        this.operationsService = operationsService;
        this.repoService = repoService;
    }

    public void MigrateAllRepos()
    {
        throw new NotImplementedException();
    }

    public void MigrateEverything()
    {
        var allRepos = repoService.Methods.GetAllReposNames();
        var allReposPath = repoService.Methods.GetAllReposPaths();
        var counts = new List<(string, int)>();
        foreach (var repo in allRepos)
        {
            var content = $"name: {repo}";
            // todo
            // repoService.Methods.CreateRepoConfig(repo, content);
        }
    }

    public void MigrateOneAddress((string Repo, string Loca) address)
    {
        throw new NotImplementedException();
    }

    public void MigrateOneFolder((string Repo, string Loca) adrTuple)
    {
        throw new NotImplementedException();
    }

    public void MigrateOneRepo(string repoName)
    {
        throw new NotImplementedException();
    }

    public void SetAgree(bool agree)
    {
        throw new NotImplementedException();
    }
}