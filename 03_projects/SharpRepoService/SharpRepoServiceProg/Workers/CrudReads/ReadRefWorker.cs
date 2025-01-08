using System.Collections.Generic;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Registration;
using NotImplementedException = System.NotImplementedException;

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
        Dictionary<string, object> refSettings = _migrate
            .GetConfigBeforeRead(refAdrTuple);

        if (refGuid != settings[ConfigKeys.RefGuid].ToString())
        {
            _guidWorker.GetAdrTupleByGuid(refAdrTuple.RefRepo, refGuid);
        }
        
        // body
        item = _multi.GetItem(refAdrTuple);

        return item;
    }

    private void TryInitialize()
    {
        _multi = MyBorder.MyContainer.Resolve<ReadMultiWorker>();
    }
}
