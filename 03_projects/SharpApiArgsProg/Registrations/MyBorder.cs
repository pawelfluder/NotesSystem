using System.Reflection;
using SharpContainerProg.AAPublic;

namespace SharpApiArgsProg.Registrations;

internal static class MyBorder
{
    // OUT-BORDER
    public static IContainer4 OutContainer => RegistrationBox.Registration
        .OutContainer;
    public static bool IsRegistered { get; } = RegistrationBox.Registration
        .Start(IsRegistered);
    
    // MY-BORDER
    private static string AssemblyName => Assembly
        .GetExecutingAssembly().FullName ?? string.Empty;
    public static IContainer4 MyContainer =
        ContainerService.MyContainer(
            AssemblyName);
}

internal static class RegistrationBox
{
    public static RegistrationBase Registration { get; set; }
}
