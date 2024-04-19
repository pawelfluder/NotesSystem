using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SharpButtonActionsProg.Workers
{
    public class WindowsWorker
    {
        static char space = ' ';

        private bool IsMyOsSystem()
        {
            var result = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            return result;
        }

        public void TryOpenFolder(string path)
        {
            if (!IsMyOsSystem()) { return; }

            var programPath = "explorer.exe";
            var windowsFormatPath = Path.GetFullPath(path);
            Process.Start(programPath, windowsFormatPath);
        }

        public void TryOpenFile(string path)
        {
            if (!IsMyOsSystem()) { return; }

            var programPath = @"C:\Program Files\Notepad++\notepad++.exe";
            var windowsFormatPath = Path.GetFullPath(path);
            Process.Start(programPath, windowsFormatPath);
        }

        public void Run(string[] args)
        {
            //var fileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            //var fileName = @"C:\Program Files\Mozilla Firefox\firefox.exe";
            //var arguments = "D:\02_Xampp\htdocs\Notki\01\02\lista.txt";

            var fileName = @"C:\Program Files\Notepad++\notepad++.exe";

            //var arguments = @"https://facebook.com";
            //var arguments = "https://www.google.com";

            string arguments = string.Empty;


            var argsList = args.Any() ? args.ToList() : new List<string>();


            if (argsList.Count == 0)
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                Process.Start("explorer.exe", currentDirectory);
            }
            else if (argsList.Count == 1)
            {
                arguments = args[0];

                if (Directory.Exists(arguments))
                {
                    Process.Start("explorer.exe", arguments);
                }
                else
                {
                    Process process = new Process();
                    process.StartInfo = new ProcessStartInfo()
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        StandardOutputEncoding = System.Text.Encoding.UTF8,
                        FileName = fileName,
                        Arguments = arguments,
                    };
                    process.Start();
                }
            }
        }
    }
}
