﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpNotesMigrationProg.AAPublic;
using SharpNotesMigrationProg.Service;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SharpRepoServiceProg.AAPublic;

namespace SharpNotesMigrationProg.Migrations;

internal class Migrator02 : IMigrator, IMigrator02
{
    private readonly IOperationsService operationsService;
    private readonly IRepoService repoService;

    public Migrator02(
        IOperationsService operationsService,
        IRepoService repoService)
    {
        this.operationsService = operationsService;
        this.repoService = repoService;
    }

    public void MigrateEverything()
    {
        List<string> allRepoNames = repoService.Methods.GetAllReposNames();
        var counts = new List<(string, int)>();
        foreach (string repoName in allRepoNames)
        {
            var address = (repoName, "");
            var foundAddressList = repoService.Methods.GetAllRepoAddresses((repoName, ""));
            //var repoName = Path.GetFileName(repoName);    
            var i = 0;
            foreach (var foundAddress in foundAddressList)
            {
                i++;
                var success = repoService.Methods.TryGetConfigLines(foundAddress, out var lines);
                if (!success)
                {
                    var deleted = TryCorrect(foundAddress);
                    if (deleted)
                    {
                        continue;
                    }
                }
                var configLines = repoService.Methods.GetConfigLines(foundAddress);
                if (configLines.Count() != 1)
                {
                    TryCorrect2(foundAddress);
                }
                var name = configLines[0];
                Console.WriteLine(i + "; " + repoName + "; " + foundAddress.Loca + "; " + name);
                counts.Add((repoName, i));
            }
        }
    }

    private void TryCorrect2((string Repo, string Loca) address)
    {
        var configLines = repoService.Methods.GetConfigLines(address);
        var notEmptyLines = configLines.Where(x => x != "");
        if (notEmptyLines.Count() == 1)
        {
            // todo
            //repoService.Methods.CreateConfig(address, new List<string> { Path.GetFileNameWithoutExtension(notEmptyLines.First()) });
        }
        else
        { }
    }

    private bool TryCorrect((string Repo, string Loca) address)
    {
        var path = repoService.Methods.GetElemPath(address);
        var files = Directory.GetFiles(path);
        var directories = Directory.GetDirectories(path);
        var indexFolders = directories
            .Where(x => operationsService.Index.IsCorrectIndex(Path.GetFileName(x)));
        var pdf = files.SingleOrDefault(x => Path.GetExtension(x) == ".pdf");
        var nameFile = files.SingleOrDefault(x => Path.GetFileName(x) == "name.txt");
        var ext = files.Select(x => Path.GetExtension(x));
        var indexPhp = files.SingleOrDefault(x => Path.GetFileName(x) == "index.php");
        var lista = files.SingleOrDefault(x => Path.GetFileName(x) == "lista.txt");

        // todo
        //if (pdf != null &&
        //    indexPhp != null)
        //{
        //    var configLines = new List<string> { Path.GetFileNameWithoutExtension(pdf) };
        //    repoService.Methods.CreateConfig(address, configLines);
        //    return false;
        //}

        //if (indexPhp != null &&
        //lista != null &&
        //    files.Count() == 1)
        //{
        //    var configLines = new List<string> { Path.GetFileNameWithoutExtension("??") };
        //    repoService.Methods.CreateConfig(address, configLines);
        //    return false;
        //}

        //if (indexPhp == null &&
        //    lista == null &&
        //    indexFolders.Count() > 0)
        //{
        //    var configLines = new List<string> { Path.GetFileNameWithoutExtension("??") };
        //    repoService.Methods.CreateConfig(address, configLines);
        //    File.Copy("Old/FolderIndexPhp/index.php", path + "/" + "index.php");
        //    return false;
        //}

        //if (indexPhp != null &&
        //    nameFile == null)
        //{
        //    var configLines = new List<string> { Path.GetFileNameWithoutExtension("??") };
        //    repoService.Methods.CreateConfig(address, configLines);
        //    return false;
        //}

        if (files.Count() == 0 &&
            directories.Count() == 0)
        {
            try
            {
                Directory.Delete(path);
            }
            catch { }
            return true;
        }

        return false;
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

    public void MigrateAllRepos()
    {
        throw new NotImplementedException();
    }

    public void SetAgree(bool agree)
    {
        throw new NotImplementedException();
    }
}