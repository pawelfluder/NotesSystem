using System.Collections.Generic;
using System.Linq;
using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.Workers.System;

namespace SharpRepoServiceProg.Workers.CrudReads;

public class ReadWorkerBase
{
    internal readonly IFileService _fileService;
    internal readonly CustomOperationsService _customOperations;
    internal readonly IYamlOperations _yamlOperations;
    
    internal readonly PathWorker _path;
    internal readonly BodyWorker _body;
    internal readonly ConfigWorker _config;
    internal readonly SystemWorker _system;
    internal readonly MemoryWorker _memory;
    internal readonly ReadAddressWorker _address;

    internal readonly MigrationWorker _migrate;
    internal readonly ReadManyWorker _readMany;
    public object ErrorValue { get; internal set; }

    public ReadWorkerBase()
    {
        _fileService = MyBorder.OutContainer.Resolve<IFileService>();
        _customOperations = MyBorder.MyContainer.Resolve<CustomOperationsService>();
        _yamlOperations = _fileService.Yaml.Custom03;
        
        _system = MyBorder.MyContainer.Resolve<SystemWorker>();
        _path = MyBorder.MyContainer.Resolve<PathWorker>();
        _config = MyBorder.MyContainer.Resolve<ConfigWorker>();
        _body = MyBorder.MyContainer.Resolve<BodyWorker>();
        _memory = MyBorder.MyContainer.Resolve<MemoryWorker>();
        _address = MyBorder.MyContainer.Resolve<ReadAddressWorker>();
        
        _migrate = MyBorder.MyContainer.Resolve<MigrationWorker>();
        _readMany = MyBorder.MyContainer.Resolve<ReadManyWorker>();
    }
    
    // read; config
    // public (string, string) GetAdrTupleByName(
    //     (string Repo, string Loca) adrTuple,
    //     string name)
    // {
    //     var items = _readMany.ListOfItemsWithConfig(adrTuple);
    //     var found = items.SingleOrDefault(x => x.Name.ToString() == name);
    //     if (found == null)
    //     {
    //         return default;
    //     }
    //
    //     var foundAdrTuple = _customOperations.UniAddress
    //         .CreateAddressFromString(found.Address);
    //     return foundAdrTuple;
    // }
    
    protected ItemModel TrySetAddress(
        ItemModel item,
        (string Repo, string Loca) adrTuple)
    {
        if (string.IsNullOrEmpty(item.Address))
        {
            string address = _customOperations.UniAddress
                .CreateAddresFromAdrTuple(adrTuple);
            item.Address = address;
        }
        
        return item;
    }
}
