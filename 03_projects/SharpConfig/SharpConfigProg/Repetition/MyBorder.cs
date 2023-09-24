using Unity;

namespace SharpConfigProg.Repetition
{
    internal static class MyBorder
    {
        private static Registration registration = new Registration();
        private static UnityContainer container = registration.Start();

        public static Registration Registration => registration;
        public static UnityContainer Container => container;
    }
}
