using System;
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

    public bool IfMineGetItem(
        ref ItemModel item,
        (string Repo, string Loca) refItemAdrTuple,
        UniType uniType)
    {
        if (_myType != uniType) { return false; }
        TryInitialize();
        
        // ref item config
        Dictionary<string, object> refItemSettings = _migrate
             .GetConfigBeforeRef(refItemAdrTuple);
        string realAddress = refItemSettings[ConfigKeys.RefAddress].ToString();
        string realGuidFromRefItem = refItemSettings[ConfigKeys.RefGuid].ToString();
        
        // real address
        (string RefRepo, string RefLoca) realAdrTuple = _operations
            .UniAddress.CreateAddressFromString(realAddress);
        
        // real config
        Dictionary<string, object> realSettings = _migrate
            .GetConfigBeforeRead(realAdrTuple);
        string realGuidStr = realSettings[ConfigKeys.Id].ToString();
        
        if (realGuidFromRefItem != realGuidStr)
        {
            Guid realGuid = Guid.Parse(realGuidStr);
            bool isFound = _guidWorker.GetAdrTupleByGuid(
                realAdrTuple.RefRepo,
                realGuid,
                out var foundAdrTuple);
            if (isFound)
            {
                realAdrTuple = foundAdrTuple;
                var foundAddress = _operations
                    .UniAddress.CreateAddresFromAdrTuple(foundAdrTuple);
                refItemSettings[ConfigKeys.RefAddress] = foundAddress;
                _config.PutConfig(refItemAdrTuple, refItemSettings);
            }
        }
        
        // body
        bool s02 = _multi.GetItem(
            ref item,
            realAdrTuple);
        return s02;
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
        (string RefRepo, string RefLoca) refAdrTuple = _operations
            .UniAddress.CreateAddressFromString(refAddress);
        
        // ref config
        Dictionary<string, object> realSettings = _migrate
            .GetConfigBeforeRead(refAdrTuple);

        string realGuidStr = realSettings[ConfigKeys.Id].ToString();
        if (refGuid != realGuidStr)
        {
            Guid realGuid = Guid.Parse(realGuidStr);
            bool isFound = _guidWorker.GetAdrTupleByGuid(
                refAdrTuple.RefRepo,
                realGuid,
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
