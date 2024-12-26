using System;
using System.Collections.Generic;
using System.IO;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Names;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.Workers.System;

namespace SharpRepoServiceProg.Workers.CrudReads;

public class MigrationWorker
{
    private readonly ConfigWorker _config;
    private readonly PathWorker _path;
    private readonly CustomOperationsService _customOperations;

    public MigrationWorker()
    {
        _customOperations = MyBorder.MyContainer.Resolve<CustomOperationsService>();
        _config = MyBorder.MyContainer.Resolve<ConfigWorker>();
        _path = MyBorder.MyContainer.Resolve<PathWorker>();
    }
    
    public ItemModel GetItemWithConfig(
        (string Repo, string Loca) adrTuple)
    {
        ItemModel item = new();
        item.Settings = GetConfigBeforeRead(adrTuple);
        return item;
    }

    public Dictionary<string, object> GetConfigBeforeWrite(
        Dictionary<string, object> config,
        (string Repo, string Loca) adrTuple)
    {
        TryMigrateConfig(config, adrTuple);
        return config;
    }

    public Dictionary<string, object> GetConfigBeforeRead(
        (string Repo, string Loca) adrTuple)
    {
        var config = _config.GetConfigDictionary(adrTuple);
        TryMigrateConfigAndSave(config, adrTuple);
        return config;
    }
    
    private bool TryMigrateConfigAndSave(
        Dictionary<string, object> settings,
        (string Repo, string Loca) adrTuple)
    {
        bool wasMigrated = false;
        bool saveNeeded = TryMigrateConfig(settings, adrTuple);
        if (saveNeeded)
        {
            _config.PutConfig(adrTuple, settings);
            wasMigrated = true;
        }
        return wasMigrated;
    }

    private bool TryMigrateConfig(
        Dictionary<string, object> settings,
        (string Repo, string Loca) adrTuple)
    {
        string newAddress = _customOperations.UniAddress
            .CreateAddresFromAdrTuple(adrTuple);
        settings[FieldsForUniItem.Address] = newAddress;
        bool saveNeeded = false;
        if (!settings.ContainsKey(FieldsForUniItem.Id))
        {
            settings[FieldsForUniItem.Id] = Guid.NewGuid().ToString();
            saveNeeded = true;
        }
        if (!settings.ContainsKey(FieldsForUniItem.Type))
        {
            string type = AssumeType(adrTuple);
            settings[FieldsForUniItem.Type] = type;
            saveNeeded = true;
        }
        if (!settings.ContainsKey(FieldsForUniItem.Address)
            || settings[FieldsForUniItem.Address] != newAddress)
        {
            settings[FieldsForUniItem.Address] = newAddress;
            saveNeeded = true;
        }
        return saveNeeded;
    }
    
    public string AssumeType(
        (string repo, string loca) adrTuple)
    {
        string contentFilePath = _path.GetBodyPath(adrTuple);
        if (File.Exists(contentFilePath))
        {
            return UniItemTypes.Text;
        }

        return UniItemTypes.Folder;
    }
}
