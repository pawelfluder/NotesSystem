using System;
using System.Collections.Generic;
using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Names;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.Workers.System;

namespace SharpRepoServiceProg.Workers.Crud;

public class WriteFolderWorker
{
    private readonly IFileService fileService;
    private readonly ReadWorker rw;
    private readonly PathWorker pw;
    private readonly ConfigWorker cw;
    private readonly BodyWorker bw;
    private readonly SystemWorker sw;
    private OperationsService _operationsService;

    public WriteFolderWorker(
        IFileService fileService)
    {
        this.fileService = fileService;
        _operationsService = MyBorder.Container.Resolve<OperationsService>();

        rw = MyBorder.Container.Resolve<ReadWorker>();
        pw = MyBorder.Container.Resolve<PathWorker>();
        cw = MyBorder.Container.Resolve<ConfigWorker>();
        bw = MyBorder.Container.Resolve<BodyWorker>();
        sw = MyBorder.Container.Resolve<SystemWorker>();
    }

    public ItemModel Put(
        string name,
        (string Repo, string Loca) adrTuple)
    {
        var item = new ItemModel();

        // config
        item.Settings = new Dictionary<string, object>()
        {
            { FieldsForUniItem.Id, Guid.NewGuid().ToString() },
            { FieldsForUniItem.Type, ItemTypeNames.Folder },
            { FieldsForUniItem.Folder, name }
        };
        
        Put(item);
        return item;
    }
    
    internal ItemModel Put(ItemModel item)
    {
        // directory
        sw.CreateDirectoryIfNotExists(item.AdrTuple);

        // config
        cw.CreateConfig(item.AdrTuple, item.Settings);

        return item;
    }

    public (string Repo, string Loca) Post(
        string name,
        (string Repo, string Loca) adrTuple)
    {
        var foundAdrTuple = rw.GetAdrTupleByName(adrTuple, name);
        if (foundAdrTuple != default)
        {
            Put(name, foundAdrTuple);
            return foundAdrTuple;
        }

        var lastIndex = rw.GetFolderLastNumber(adrTuple);
        var newIndex = lastIndex + 1;
        var newIndexString = _operationsService.Index.IndexToString(newIndex);

        var newAdrTuple = _operationsService.Index.AdrTupleJoinLoca(adrTuple, newIndexString);
        Put(name, newAdrTuple);
        return newAdrTuple;
    }

    internal ItemModel InternalPost(
        string name,
        (string Repo, string Loca) adrTuple)
    {
        ItemModel item = null;
        var foundAdrTuple = rw.GetAdrTupleByName(adrTuple, name);
        if (foundAdrTuple != default)
        {
            item = rw.GetItemConfig(adrTuple);
            Put(name, foundAdrTuple);
            return item;
        }

        var lastIndex = rw.GetFolderLastNumber(adrTuple);
        var newIndex = lastIndex + 1;
        var newIndexString = _operationsService.Index.IndexToString(newIndex);

        var newAdrTuple = _operationsService.Index.AdrTupleJoinLoca(adrTuple, newIndexString);
        item = PrepareItem(name, newAdrTuple);
        Put(item);
        return item;
    }

    private ItemModel PrepareItem(
        string name,
        (string Repo, string Loca) adrTuple)
    {
        var item = new ItemModel();

        // config
        var settings = new Dictionary<string, object>()
        {
            { FieldsForUniItem.Id, Guid.NewGuid().ToString() },
            { FieldsForUniItem.Type, ItemTypeNames.Folder },
            { FieldsForUniItem.Name, name },
        };
        cw.AddSettingsToModel(item, adrTuple, settings);
        return item;
    }

    
}