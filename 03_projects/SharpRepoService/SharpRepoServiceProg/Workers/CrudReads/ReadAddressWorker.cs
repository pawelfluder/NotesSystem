using System.Collections.Generic;
using System.Linq;
using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registrations;
using SharpRepoServiceProg.Workers.System;

namespace SharpRepoServiceProg.Workers.CrudReads;

internal class ReadAddressWorker
{
    private readonly CustomOperationsService _customOperationsService;
    private readonly ReadManyWorker _readMany;
    private readonly PathWorker _path;
    private readonly SystemWorker _system;
    private readonly ReadHelper _helper;

    public ReadAddressWorker()
    {
        _customOperationsService = MyBorder.MyContainer.Resolve<CustomOperationsService>();
        _path = MyBorder.MyContainer.Resolve<PathWorker>();
        _system = MyBorder.MyContainer.Resolve<SystemWorker>();
        _helper = MyBorder.MyContainer.Resolve<ReadHelper>();
        _readMany = MyBorder.MyContainer.Resolve<ReadManyWorker>();
    }
	
    public (string Repo, string Loca) GetAdrTupleByName(
        (string Repo, string Loca) adrTuple,
        string name)
    {
        List<ItemModel> items = _readMany
            .ListOfOnlyConfigItems(adrTuple);
        ItemModel found = items.SingleOrDefault(x => 
            x.Name.ToString() == name);
        if (found == null)
        {
            return default;
        }

        (string, string) foundAdrTuple = _customOperationsService.UniAddress
            .CreateAddressFromString(found.Address);
        return foundAdrTuple;
    }
	
    // read; config
    public (string, string) GetAdrTupleBySequenceOfNames(
        (string Repo, string Loca) adrTuple,
        params string[] names)
    {
        foreach (var name in names)
        {
            adrTuple = GetAdrTupleByName(adrTuple, name);

            if (adrTuple == default)
            {
                return default;
            }
        }

        return adrTuple;
    }
    
    public List<(string, string)> GetSubAdrTuples(
        (string Repo, string Loca) adrTuple)
    {
        string itemPath = _path.GetItemPath(adrTuple);
        string[] dirs = _system.GetDirectories(itemPath);
        List<(string Repo, string)> subAddresses = dirs
            .Select(x => (adrTuple.Repo, _helper
                .SelectDirToSection(adrTuple.Loca, x)))
            .ToList();
        return subAddresses;
    }
}
