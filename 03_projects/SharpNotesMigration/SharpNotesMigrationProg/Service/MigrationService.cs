using System;
using System.Collections.Generic;
using System.Linq;
using SharpConfigProg.AAPublic;
using SharpConfigProg.Service;
using SharpNotesMigrationProg.Migrations;
using SharpRepoServiceProg.AAPublic;
using SharpFileServiceProg.AAPublic;
using SharpNotesMigrationProg.AAPublic;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpNotesMigrationProg.Service;

public class MigrationService : IMigrationService
{
    private readonly IFileService _fileService;
    private readonly IRepoService _repoService;
    private readonly IConfigService configService;
    private readonly IOperationsService _operationsService;

    private readonly List<IMigrator> migratorsList;

    public MigrationService(
        IOperationsService operationsService,
        IRepoService repoService)
    {
        _operationsService = operationsService;
        _repoService = repoService;
        _fileService = _operationsService.GetFileService();

        migratorsList = new List<IMigrator>()
        {
            new Migrator03(_operationsService, repoService),
            new Migrator04(_operationsService, repoService),
            new Migrator05(_operationsService, repoService),
        };
    }

    // todo move to OperationsService
    private IEnumerable<Type> Method()
    {
        var type = typeof(IMigrator);
        IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));
        return types;
    }

    public void MigrateOneAddress(
        Type migratorType,
        (string Repo, string Loca) address,
        bool agree)
    {
        var found = migratorsList.SingleOrDefault(x => x.GetType().GetInterfaces().Contains(migratorType));

        if (found != null)
        {
            found.SetAgree(agree);
            found.MigrateOneAddress(address);
        }
    }

    public void MigrateOneFolder(
        Type migratorType,
        (string Repo, string Loca) address,
        bool agree)
    {
        var found = migratorsList.SingleOrDefault(x => x.GetType().GetInterfaces().Contains(migratorType));

        if (found != null)
        {
            found.SetAgree(agree);
            found.MigrateOneFolder(address);
        }            
    }

    public void MigrateOneRepo(
        Type migratorType,
        string repoName,
        bool agree)
    {
        var found = migratorsList.SingleOrDefault(x => x.GetType() == migratorType);

        if (found != null)
        {
            found.MigrateOneRepo(repoName);
        }
    }

    public void MigrateAllRepos(Type migratorType)
    {
        var found = migratorsList.SingleOrDefault(x => x.GetType() == migratorType);

        if (found != null)
        {
            found.MigrateAllRepos();
        }
    }
}