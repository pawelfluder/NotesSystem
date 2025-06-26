using SharpContainerProg.AAPublic;
using OutBorder01 = SharpSetup01Prog.AAPublic.OutBorder;

namespace BlazorUniSystemCore;

public static class Program
{
    public static void Main(string[] args)
    {
        AppFasade app = new AppFasade();
        ContainerService.SetOutContainer(app.Container);
        OutBorder01.DefaultPreparer().Prepare();
        app.Start();
        Console.ReadLine();
    }
}
