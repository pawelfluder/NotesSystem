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
        MyBorder.MyContainer.RegisterByFunc(
            () => new CustomOperationsService());
        ;
        MyBorder.MyContainer.RegisterByFunc(
            () => new PathWorker());

        SystemWorker systemWorker = new();
        MyBorder.MyContainer.RegisterByFunc(() => systemWorker);

        BodyWorker bodyWorker = new();
        MyBorder.MyContainer.RegisterByFunc(() => bodyWorker);

        ConfigWorker configWorker = new();
        MyBorder.MyContainer.RegisterByFunc(() => configWorker);

        MemoryWorker memoWorker = new();
        MyBorder.MyContainer.RegisterByFunc(() => memoWorker);

        ReadWorker itemWorker = new();
        MyBorder.MyContainer.RegisterByFunc(() => itemWorker);

        JsonWorker jsonWorker = new JsonWorker();
        MyBorder.MyContainer.RegisterByFunc(() => jsonWorker);

        Func<WriteFolderWorker> writeFolderFunc = () => { return new(); };
        MyBorder.MyContainer.RegisterByFunc(writeFolderFunc);
        
        Func<WriteTextWorker> writeTextFunc = () => { return new(); };
        MyBorder.MyContainer.RegisterByFunc(writeTextFunc);
    }
}
