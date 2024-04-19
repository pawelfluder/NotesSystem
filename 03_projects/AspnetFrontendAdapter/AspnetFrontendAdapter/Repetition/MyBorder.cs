using Unity;

namespace AspnetFrontendAdapterProg.Repetition
{
    internal static class MyBorder
    {
        private static UnityContainer container = new Registration().Start();
        public static UnityContainer Container => container;
    }
}
