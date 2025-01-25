using System.Collections.Generic;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Registration;

namespace SharpRepoServiceProg.Workers.CrudReads;
internal class ReadRefWorker : ReadWorkerBase
{
    private readonly UniType _myType = UniType.Ref;
    private readonly GuidWorker _guidWorker;
    private ReadMultiWorker _multi;

    public ReadRefWorker()
    {
        _guidWorker = MyBorder.MyContainer.Resolve<GuidWorker>();
    }

    public ItemModel TryGetItem(
        ItemModel item,
        (string Repo, string Loca) refItemAdrTuple,
        UniType uniType)
    {
        TryInitialize();
        if (_myType != uniType) { return item; }
        
        // ref item config
        Dictionary<string, object> refItemSettings = _migrate
             .GetConfigBeforeRef(refItemAdrTuple);
        string realAddress = refItemSettings[ConfigKeys.RefAddress].ToString();
        string realGuidFromRefItem = refItemSettings[ConfigKeys.RefGuid].ToString();
        
        // real address
        (string RefRepo, string RefLoca) realAdrTuple = _customOperations
            .UniAddress.CreateAddressFromString(realAddress);
        
        // real config
        Dictionary<string, object> realSettings = _migrate
            .GetConfigBeforeRead(realAdrTuple);

        string realGuid = realSettings[ConfigKeys.Id].ToString();
        if (realGuidFromRefItem != realGuid)
        {
            bool isFound = _guidWorker.GetAdrTupleByGuid(
                realAdrTuple.RefRepo,
                realGuidFromRefItem,
                out var foundAdrTuple);
            if (isFound)
            {
                realAdrTuple = foundAdrTuple;
                var foundAddress = _customOperations
                    .UniAddress.CreateAddresFromAdrTuple(foundAdrTuple);
                refItemSettings[ConfigKeys.RefAddress] = foundAddress;
                _config.PutConfig(refItemAdrTuple, refItemSettings);
            }
        }
        
        // body
        item = _multi.GetItem(realAdrTuple);

        return item;
    }
    
    public ItemModel TryGetItemBody(
        ItemModel item,
        (string Repo, string Loca) adrTuple,
        UniType uniType)
    {
        TryInitialize();
        if (_myType != uniType) { return item; }
        
        // config
        Dictionary<string, object> settings = _migrate
            .GetConfigBeforeRef(adrTuple);
        string refAddress = settings[ConfigKeys.RefAddress].ToString();
        string refGuid = settings[ConfigKeys.RefGuid].ToString();
        
        // address
        (string RefRepo, string RefLoca) refAdrTuple = _customOperations
            .UniAddress.CreateAddressFromString(refAddress);
        
        // ref config
        Dictionary<string, object> realSettings = _migrate
            .GetConfigBeforeRead(refAdrTuple);

        string realGuid = realSettings[ConfigKeys.Id].ToString();
        if (refGuid != realGuid)
        {
            bool isFound = _guidWorker.GetAdrTupleByGuid(
                refAdrTuple.RefRepo,
                refGuid,
                out var foundAdrTuple);
            if (isFound)
            {
                refAdrTuple = foundAdrTuple;
                // write to config
            }
        }
        
        // body
        item = _multi.GetItemBody(refAdrTuple);

        return item;
    }

    private void TryInitialize()
    {
        _multi = MyBorder.MyContainer.Resolve<ReadMultiWorker>();
    }
}
