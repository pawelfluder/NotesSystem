using SharpContainerProg.AAPublic;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Workers.Crud;
using SharpRepoServiceProg.Workers.Public;
using SharpRepoServiceProg.Workers.System;

namespace SharpRepoServiceProg.Registration;

internal class Registration : RegistrationBase
{
    public override void Registrations()
    {
        var pathWorker = new PathWorker();
        RegisterByFunc(() => pathWorker);

        var systemWorker = new SystemWorker();
        RegisterByFunc(() => systemWorker);

        var bodyWorker = new BodyWorker();
        RegisterByFunc(() => bodyWorker);

        var configWorker = new ConfigWorker();
        RegisterByFunc(() => configWorker);

        var memoWorker = new MemoryWorker();
        RegisterByFunc(() => memoWorker);

        var itemWorker = new ReadWorker();
        RegisterByFunc(() => itemWorker);

        var jsonWorker = new JsonWorker();
        RegisterByFunc(() => jsonWorker);

        var operationsService = new OperationsService();
        RegisterByFunc(() => operationsService);
    }
}