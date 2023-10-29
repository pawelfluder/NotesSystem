using SharpContainerProg.AAPublic;

namespace SharpConfigProg.Register
{
    internal static class MyBorder
    {
        public static bool IsRegistered = true;
        public static Registration Registration => new Registration();
        public static IContainer Container => Registration
            .Start(ref IsRegistered);
    }
}
