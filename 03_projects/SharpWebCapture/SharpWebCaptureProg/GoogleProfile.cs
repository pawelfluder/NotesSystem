using System.Runtime.InteropServices;

namespace SharpWebCaptureProg
{
    internal class GoogleProfile
    {
        public string TryGetUserDataDir()
        {
            // --user-data-dir=

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "/Users/pawelfluder/Library/Application Support/Google/Chrome";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "C:\\03_synch\\02_programs_portable\\02_chrome\\01_pawelfluder\\Data\\profile";
            }

            return default;
        }

        public string TryGetProfileDir()
        {
            // --profile-directory

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "Profile 2";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return default;
            }

            return default;
        }
    }
}