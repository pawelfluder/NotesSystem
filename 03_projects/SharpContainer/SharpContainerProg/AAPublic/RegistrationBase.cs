using System.Reflection;
using Unity.Injection;

namespace SharpContainerProg.AAPublic;

public abstract class RegistrationBase
{
    public IContainer4 OutContainer =
        ContainerService.OutContainer;
    
    private bool registrationStarted;

    public bool Start(bool isRegistered)
    {
        if (!registrationStarted
            && !isRegistered)
        {
            registrationStarted = true;
            Registrations();
            isRegistered = true;
            registrationStarted = false;
        }

        return true;
    }

    public abstract void Registrations();
}
