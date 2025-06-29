using System.Reflection;
using SharpContainerProg.AAPublic;

namespace SharpIdentityProg.Registrations;

internal static class MyBorder
{
    static MyBorder()
    {
        Registration = new Registration();
        IsRegistered = Registration.Start(IsRegistered);
    }
    public static Registration Registration { get; }
    public static bool IsRegistered { get; }
    public static IContainer4 OutContainer => Registration.OutContainer;
    
    // MyContainer
    private static string AssemblyName => Assembly
        .GetExecutingAssembly().FullName ?? string.Empty;
    public static IContainer4 MyContainer => ContainerService.MyContainer(AssemblyName);
}
