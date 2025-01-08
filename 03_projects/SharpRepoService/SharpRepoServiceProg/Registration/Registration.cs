using SharpContainerProg.AAPublic;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Workers.AAPublic;
using SharpRepoServiceProg.Workers.CrudReads;
using SharpRepoServiceProg.Workers.CrudWrites;
using SharpRepoServiceProg.Workers.System;

namespace SharpRepoServiceProg.Registration;

internal class Registration : RegistrationBase
{
    public override void Registrations()
    {
        RegisterBase();
        
        RegisterRead();

        RegisterWrite();

        MyBorder.MyContainer.RegisterByFunc(
            () => new JsonWorker());
    }

    private static void RegisterWrite()
    {
        MyBorder.MyContainer.RegisterByFunc(
            () => new WriteFolderWorker());
        
        MyBorder.MyContainer.RegisterByFunc(
            () => new WriteTextWorker());
        
        MyBorder.MyContainer.RegisterByFunc(
            () => new WriteMultiWorker());
    }

    private static void RegisterRead()
    {
        MyBorder.MyContainer.RegisterByFunc(
            () => new ReadAddressWorker());
        
        // base; PathWorker
        // base; BodyWorker
        // base; ConfigWorker>();
        // base; SystemWorker>();
        // base; MemoryWorker>();
        // base; MigrationWorker>();
        // base; ReadManyWorker>();
        MyBorder.MyContainer.RegisterByFunc(
            () => new ReadFolderWorker());
        
        MyBorder.MyContainer.RegisterByFunc(
            () => new ReadTextWorker());
        
        MyBorder.MyContainer.RegisterByFunc(
            () => new ReadRefWorker());

        // ReadFolderWorker
        // ReadTextWorker
        // base
        MyBorder.MyContainer.RegisterByFunc(
            () => new ReadMultiWorker());
    }

    private static void RegisterBase()
    {
        // nothing
        MyBorder.MyContainer.RegisterByFunc(
            () => new CustomOperationsService());
        
        // CustomOperationsService
        MyBorder.MyContainer.RegisterByFunc(
            () => new PathWorker());
        
        // PathWorker
        MyBorder.MyContainer.RegisterByFunc(
            () => new SystemWorker());
        
        MyBorder.MyContainer.RegisterByFunc(
            () => new ReadHelper());
        
        // PathWorker
        MyBorder.MyContainer.RegisterByFunc(
            () => new MemoryWorker());
        
        // BodyWorker
        MyBorder.MyContainer.RegisterByFunc(
            () => new BodyWorker());
        
        // CustomOperationsService
        // FileService
        // PathWorker
        // SystemWorker
        MyBorder.MyContainer.RegisterByFunc(
            () => new ConfigWorker());
        
        // CustomOperationsService
        // ConfigService
        // PathWorker
        MyBorder.MyContainer.RegisterByFunc(
            () => new MigrationWorker());
        
        // CustomOperationsService
        // FileService
        MyBorder.MyContainer.RegisterByFunc(
            () => new ReadManyWorker());
        
        MyBorder.MyContainer.RegisterByFunc(
            () => new GuidWorker());
    }
}
