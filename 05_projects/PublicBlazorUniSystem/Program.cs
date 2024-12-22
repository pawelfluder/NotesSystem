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
        app.Start();
        Console.ReadLine();
    }
}
