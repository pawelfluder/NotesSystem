using System;
using System.Collections.Generic;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Names;
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
    private readonly OperationsService _operationsService;

    public DeleteWorker()
    {
        _rw = MyBorder.Container.Resolve<ReadWorker>();
        _bw = MyBorder.Container.Resolve<BodyWorker>();
        _cw = MyBorder.Container.Resolve<ConfigWorker>();
        _sw = MyBorder.Container.Resolve<SystemWorker>();
        _operationsService = MyBorder.Container.Resolve<OperationsService>();
    }

    public void Delete(
        (string Repo, string Loca) adrTuple)
    {
    }
}