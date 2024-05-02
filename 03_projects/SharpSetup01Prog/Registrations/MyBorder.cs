using SharpContainerProg.AAPublic;
using SharpSetup21ProgPrivate.Registrations;

namespace SharpRepoServiceProg.Registration
{
    internal static class MyBorder
    {
        public static bool IsRegistered;
        public static DefaultRegistration Registration = new DefaultRegistration();
        public static IContainer Container => Registration.Start(ref IsRegistered);
    }
}