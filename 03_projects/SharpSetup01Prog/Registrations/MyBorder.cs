using System.Reflection;
using SharpContainerProg.AAPublic;

namespace SharpSetup01Prog.Registrations;

internal static class MyBorder
{
    private static string AssemblyName => Assembly
        .GetExecutingAssembly().FullName;
    
    public static IContainer4 MyContainer =
        ContainerService.MyContainer(
            AssemblyName);
    public static DefaultRegistration Registration = new DefaultRegistration();
    public static bool IsRegistered = Registration.Start(IsRegistered);
    public static IContainer4 OutContainer => Registration.OutContainer;
}
