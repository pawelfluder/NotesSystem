using SharpContainerProg.AAPublic;

namespace PublicWebApi.Registrations;
internal static class MyBorder
{
    public static Registration Registration = new Registration();
    public static bool IsRegistered = Registration.Start(IsRegistered);
    public static IContainer4 OutContainer => Registration.OutContainer;
}
