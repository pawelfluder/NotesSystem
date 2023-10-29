using GoogleDocsServiceProj.Service;
using SharpConfigProg.Service;
using SharpContainerProg.AAPublic;
using SharpGoogleDriveProg.Service;
using Unity;

namespace WpfNotesSystem.Repetition
{
    internal static class MyBorder
    {
        public static bool IsRegistered = false; 
        public static Registration Registration = new Registration();
        public static IContainer Container => Registration.Start(ref IsRegistered);
    }
}
