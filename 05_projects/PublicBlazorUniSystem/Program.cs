using BlazorNotesSystem;
using BlazorUniSystem.Registrations;
using Microsoft.Extensions.DependencyInjection;
using SharpContainerProg.AAPublic;
using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using OutBorder01 = SharpSetup01Prog.AAPublic.OutBorder;

namespace BlazorUniSystem;

public static class Program
{
    public static void Main(string[] args)
    {
        AppFasade app = new AppFasade();
        ContainerService.SetOutContainer(app.Container);
        OutBorder01.GetPreparer("DefaultPreparer").Prepare();
        
        // IBackendService backend = MyBorder.OutContainer.Resolve<IBackendService>();
        // app.Builder.Services.AddSingleton<IBackendService>(backend);
        //
        // IFileService fileService = MyBorder.OutContainer.Resolve<IFileService>();
        // app.Builder.Services.AddSingleton<IFileService>(fileService);
        //
        // IOperationsService operationsService = MyBorder.OutContainer.Resolve<IOperationsService>();
        // app.Builder.Services.AddSingleton<IOperationsService>(operationsService);

        // IArgsManagerService argsManager = MyBorder.Container.Resolve<IArgsManagerService>();
        // app.Services.AddSingleton<IArgsManagerService>(argsManager);
        
        app.Start();
        Console.ReadLine();
    }
}
