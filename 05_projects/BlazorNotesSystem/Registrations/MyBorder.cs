using SharpContainerProg.AAPublic;

namespace BlazorInterAutoProj.Registrations
{
    internal static class MyBorder
    {
        public static bool IsRegistered;
        public static Registration Registration = new Registration();
        public static IContainer Container => Registration.Start(ref IsRegistered);
    }
}
