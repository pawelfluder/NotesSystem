using SharpContainerProg.Containers;
using OutBorder01 = SharpSetup01Prog.AAPublic.OutBorder;

namespace BlazorUniSystemCore.Registrations;

public class UniSystemCoreContainer
    : DefaultContainerBase
{
    public UniSystemCoreContainer(
        IServiceCollection serviceCollection)
    {
        ServiceRegister = serviceCollection;
    }

    public void SetServiceProvider(
        IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public override void RegisterMocks()
    {
        OutBorder01.GetPreparer("DefaultPreparer").Prepare();
    }
}
