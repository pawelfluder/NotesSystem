using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.Workers.System;

namespace SharpRepoServiceProg.Workers.Crud;

public class DeleteWorker
{
    private readonly PathWorker pw;
    private readonly SystemWorker _sw;
    private readonly ConfigWorker _cw;
    private readonly BodyWorker _bw;
    private readonly ReadWorker _rw;
    private readonly CustomOperationsService _customOperationsService;

    public DeleteWorker()
    {
        _rw = MyBorder.OutContainer.Resolve<ReadWorker>();
        _bw = MyBorder.OutContainer.Resolve<BodyWorker>();
        _cw = MyBorder.OutContainer.Resolve<ConfigWorker>();
        _sw = MyBorder.OutContainer.Resolve<SystemWorker>();
        _customOperationsService = MyBorder.MyContainer.Resolve<CustomOperationsService>();
    }

    public void Delete(
        (string Repo, string Loca) adrTuple)
    {
    }
}