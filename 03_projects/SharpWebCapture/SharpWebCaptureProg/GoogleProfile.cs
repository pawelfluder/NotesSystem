using System.Runtime.InteropServices;

namespace SharpWebCaptureProg
{
    // https://github.com/ultrafunkamsterdam/undetected-chromedriver/issues/1150
    // options.add_argument(r'--user-data-dir=/Users/vishruth/Library/Application Support/Google/Chrome/')
    // options.add_argument(r'--profile-directory=Profile 3')

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
                return "C:\\03_synch\\02_programs_portable\\07_pawelfluder\\Data\\profile";
                //return "C:\\03_synch\\02_programs_portable\\02_chrome\\01_pawelfluder\\Data\\profile";
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