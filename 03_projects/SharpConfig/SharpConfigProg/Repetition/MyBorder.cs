using SharpContainerProg.AAPublic;
using Unity;

namespace SharpConfigProg.Repetition
{
    internal static class MyBorder
    {
        private static Registration registration = new Registration();
        private static IContainer container = registration.Start();

        public static Registration Registration => registration;
        public static IContainer Container => container;
    }
}
