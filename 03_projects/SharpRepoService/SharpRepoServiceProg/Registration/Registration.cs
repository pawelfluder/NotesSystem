using System;
using System.Reflection;
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
        var operationsService = new CustomOperationsService();
        MyBorder.MyContainer.RegisterByFunc(() => operationsService);
        
        var pathWorker = new PathWorker();
        MyBorder.MyContainer.RegisterByFunc(() => pathWorker);

        var systemWorker = new SystemWorker();
        MyBorder.MyContainer.RegisterByFunc(() => systemWorker);

        var bodyWorker = new BodyWorker();
        MyBorder.MyContainer.RegisterByFunc(() => bodyWorker);

        var configWorker = new ConfigWorker();
        MyBorder.MyContainer.RegisterByFunc(() => configWorker);

        var memoWorker = new MemoryWorker();
        MyBorder.MyContainer.RegisterByFunc(() => memoWorker);

        var itemWorker = new ReadWorker();
        MyBorder.MyContainer.RegisterByFunc(() => itemWorker);

        var jsonWorker = new JsonWorker();
        MyBorder.MyContainer.RegisterByFunc(() => jsonWorker);

        Func<WriteFolderWorker> writeFolderFunc = () => { return new(); };
        MyBorder.MyContainer.RegisterByFunc(writeFolderFunc);
        
        Func<WriteTextWorker> writeTextFunc = () => { return new(); };
        MyBorder.MyContainer.RegisterByFunc(writeTextFunc);
    }
}
