using Microsoft.Extensions.DependencyInjection;
using SharpContainerProg.AAPublic;

namespace SharpContainerProg.Containers;

public class DefaultContainer : DefaultContainerBase
{
    public DefaultContainer()
    {
        ServiceRegister = new ServiceCollection();
    }
}
