using SharpContainerProg.Containers;

namespace BlazorNotesSystem.Registrations;

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
}
