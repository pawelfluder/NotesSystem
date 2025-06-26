using BlazorUniSystemCore;
using SharpConfigProg.AAPublic;
using SharpContainerProg.AAPublic;
using OutBorder01 = SharpSetup01Prog.AAPublic.OutBorder;

namespace BlazorUniSystem;

public static class Program
{
    public static void Main(string[] args)
    {
        AppFasade app = new AppFasade();
        ContainerService.SetOutContainer(app.Container);
        IPreparer preparer = OutBorder01.DefaultPreparer("DefaultPreparer");
        preparer.Prepare();
        app.Start();
        Console.ReadLine();
    }
}
