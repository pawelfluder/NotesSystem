using Unity;

namespace SharpNotesMigrationProg.Repetition
{
    internal static class MyBorder
    {
        private static UnityContainer container = new Registration().Start();
        public static UnityContainer Container => container;
    }
}
