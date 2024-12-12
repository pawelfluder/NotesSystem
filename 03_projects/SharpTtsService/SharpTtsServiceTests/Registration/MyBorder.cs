using System.ComponentModel;
using SharpContainerProg.AAPublic;

namespace SharpTtsServiceTests.Registration;

internal static class MyBorder
{
    public static Registration Registration = new();
    public static bool IsRegistered = Registration.Start(IsRegistered);
    public static IContainer4 OutContainer => Registration.OutContainer;
}
