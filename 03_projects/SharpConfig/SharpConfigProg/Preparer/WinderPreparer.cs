using System.Management.Automation;

namespace SharpConfigProg.Preparer
{
    internal class WinderPreparer : IPreparer.IWinderPreparer
    {
        private readonly string configFilePath;

        public WinderPreparer(
            string configFilePath)
        {
            this.configFilePath = configFilePath;
        }

        public Dictionary<string, object> Prepare()
        {
            var paths = new Dictionary<string, object>();

            var azureDevopsToken = "fyo2u7cy4qypqmoxegg2mlzon6bbf3ykti6ks7dtqsxpmhzg56fa";
            var buildSourceDir = GetEnvironmentVariable("BUILD_SOURCESDIRECTORY");
            var computerName = GetEnvironmentVariable("COMPUTERNAME");
            var agentToolsDir = GetEnvironmentVariable("AGENT_TOOLSDIRECTORY");
            var isServer = IsServer(computerName);
            var userName = GetCmdOutput("whoami");
            var isRemoteSession = isServer && buildSourceDir == string.Empty;
            var isLocal = !isServer;

            Console.WriteLine($"computerName: {computerName}");
            Console.WriteLine($"buildSourceDir: {buildSourceDir}");
            Console.WriteLine($"agentToolsDir: {agentToolsDir}");
            Console.WriteLine($"isLocal: {isLocal}");
            Console.WriteLine($"isServer: {isServer}");
            Console.WriteLine($"isRemoteSession: {isServer}");

            if (isLocal)
            {
                var synchFolderPath = "D:/01_Synchronized";
                var repoRootPaths = new List<object>
                {
                    synchFolderPath + "/" + "01_Programming_Files/0fc7da8d-3466-4964-a24c-dfc0d0fef87c",
                    synchFolderPath + "/" + "01_Programming_Files/bec4317c-2466-4e31-888d-0390780986d7",
                    synchFolderPath + "/" + "01_Programming_Files/1cd6ff7d-72c0-40f2-b05c-03f7f2ce1b4c",
                    synchFolderPath + "/" + "01_Programming_Files/c98c246f-3a07-4331-a460-bfad5e9aa541",
                    synchFolderPath + "/" + "01_Programming_Files/8c0f7763-7149-4b4d-9d6a-b28d3984552f",
                    synchFolderPath + "/" + "01_Programming_Files/ebf8d4ba-06c2-43eb-a201-4d32d13656e4",
                    synchFolderPath + "/" + "01_Programming_Files/8ce8792f-dc83-4978-a1d8-1a49c71937ec",
                    synchFolderPath + "/" + "01_Programming_Files/9558c642-7766-46ae-bf6a-af380e4c68b9",
                };
                paths = new Dictionary<string, object>()
                {
                    { "pathConfigPath", configFilePath},
                    { "winderAppDataPath", "02_appData"},
                    { "driverPath", "driver"},
                    { "browserPath", synchFolderPath + "/" + "02_Portable_Programs/11_Firefox/14_paweltinder_firefox/App/Firefox/firefox.exe"},
                    { "profilePath", synchFolderPath + "/" + "02_Portable_Programs/11_Firefox/14_paweltinder_firefox/Data/profile"},
                    { "tinderApiTokenPath", "smstoken.txt"},
                    { "tinderSqlLitePath", "D:/01_Synchronized/02_Portable_Programs/11_Firefox/14_paweltinder_firefox/Data/profile/storage/default/https+++tinder.com/ls/data.sqlite" },
                    { "repoRootPaths", repoRootPaths},
                    { "appDataPath", "D:/01_Synchronized/01_Programming_Files/8c0f7763-7149-4b4d-9d6a-b28d3984552f/02_appData"},
                    { "azureDevopsToken", azureDevopsToken},
                    { "userName", userName},
                    { "computerName", computerName},
                    { "isLocal", isLocal},
                    { "isRemoteSession", isRemoteSession},
                    { "isServer", isServer},
                };
            }

            if (isServer && isRemoteSession)
            {
                CheckBuildSourceDir(ref buildSourceDir);
                CheckAgentToolsDir(ref agentToolsDir);
            }

            if (isServer)
            {
                var repoRootPaths = new List<string>
                {
                    buildSourceDir
                };

                paths = new Dictionary<string, object>()
                {
                    { "pathConfigPath", configFilePath},
                    { "winderAppDataPath", "02_appData"},
                    { "driverPath", "driver"},
                    { "browserPath", agentToolsDir + "/" + "14_paweltinder_firefox/App/Firefox/firefox.exe"},
                    { "profilePath", agentToolsDir + "/" + "14_paweltinder_firefox/Data/profile"},
                    { "tinderApiTokenPath", "smstoken.txt"},
                    { "tinderSqlLitePath", agentToolsDir + "/" + "14_paweltinder_firefox/Data/profile/storage/default/https+++tinder.com/ls/data.sqlite" },
                    { "repoRootPaths", repoRootPaths},
                    { "appDataPath", buildSourceDir + "/" + "appData"},
                    { "azureDevopsToken", azureDevopsToken},
                    { "isLocal", isLocal},
                    { "userName", userName},
                    { "computerName", computerName},
                    { "isRemoteSession", isRemoteSession},
                    { "isServer", isServer},
                };
            }

            paths.ToList().ForEach(x => Console.WriteLine(x.Key + ": " + x.Value));
            return paths;
        }

        protected string GetCmdOutput(string cmd)
        {
            var ps = PowerShell.Create();
            ps.AddScript(cmd);
            var psObject = ps.Invoke();
            var tmp1 = psObject.Select(x => x.BaseObject).ToList();
            var result = string.Join(" ", tmp1);

            ps.Dispose();
            return result;
        }


        protected bool IsServer(string computerName)
        {
            if (computerName == "webservice")
            {
                return true;
            }
            return false;
        }

        protected string GetEnvironmentVariable(string varName)
        {
            var cmd = $"Get-Item -Path Env:{varName}";
            var ps = PowerShell.Create();
            ps.AddScript(cmd);
            var psObject = ps.Invoke();
            var tmp1 = psObject.SelectMany(x => x.Properties).ToList();
            var tmp2 = tmp1.Select(x => x.Value.ToString());
            var tmp3 = string.Join(" ", tmp2);
            var result = string.Empty;
            if (tmp3.Count() > 0)
            {
                result = tmp2.Last().Replace('\\', '/');
            }

            ps.Dispose();
            return result;
        }

        protected void CheckBuildSourceDir(ref string buildSourceDir)
        {
            if (buildSourceDir == string.Empty)
            {
                var agentsWorkPath = "C:/agent/_work";
                var dirs = Directory.GetDirectories(agentsWorkPath).ToList();
                dirs = dirs.Where(x => int.TryParse(Path.GetFileName(x), out var tmp)).ToList();
                var numbers = dirs.Select(x => int.Parse(Path.GetFileName(x))).ToList();
                var max = numbers.Max();
                var lastPath = agentsWorkPath + "/" + max + "/" + "s";
                buildSourceDir = lastPath;
            }
        }

        protected void CheckAgentToolsDir(ref string agentToolsDir)
        {
            if (agentToolsDir == string.Empty)
            {
                agentToolsDir = "C:/agent/_work/_tool";
            }
        }

        //protected void GetDefaultBrowser(ref string agentToolsDir)
        //{
        //$Computer = "MY_PC_NAME"
        //$Reg = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey('CurrentUser', $Computer)
        //$RegKey = $Reg.OpenSubKey("Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice")
        //$BrowserProgId = $RegKey.GetValue("ProgId")
        //Write - Output $BrowserProgId
        //}

        protected int GetProductType()
        {
            //$osInfo = Get-CimInstance -ClassName Win32_OperatingSystem
            //$osInfo.ProductType

            //ProductType
            //Data type: uint32
            //Access type: Read - only
            //Additional system information.
            //Work Station(1)
            //Domain Controller(2)
            //Server(3)

            var cmd1 = "cd c:/;$osInfo = Get-CimInstance -ClassName Win32_OperatingSystem; $osInfo";
            var cmd = "Get-CimClass";
            //var cmd = "pwd";
            var ps = PowerShell.Create();
            ps.AddScript(cmd);
            var result = ps.Invoke();
            var resultString = result.ToString();
            var success = int.TryParse(resultString, out int number);

            if (!success)
            {
                throw new Exception();
            }

            return number;
        }
    }
}
