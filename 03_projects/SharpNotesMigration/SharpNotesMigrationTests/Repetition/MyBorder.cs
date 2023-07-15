using SharpRepoBackendProg.Repetition;
using Unity;

namespace SharpNotesMigrationTests.Repetition
{
    internal static class MyBorder
    {
        private static UnityContainer container = new Registration().Start();
        public static UnityContainer Container => container;
    }
}
