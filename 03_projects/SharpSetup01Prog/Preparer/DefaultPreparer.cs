using SharpConfigProg.AAPublic;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Registration;
using System.Runtime.InteropServices;
using OutBorder1 = SharpFileServiceProg.AAPublic.OutBorder;

namespace SharpSetup21ProgPrivate.Preparer
{
    internal class DefaultPreparer : IPreparer
    {
        private readonly IGoogleCredentialWorker credentials;
        private IConfigService configService;
        private Dictionary<string, object> settingsDict;
        private IFileService fileService;

        public DefaultPreparer(IFileService fileService)
        {
            credentials = fileService.Credentials;
            settingsDict = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Prepare()
        {
            fileService = OutBorder1.FileService();
            var repoRootPaths = GetRepoSearchPaths();
            settingsDict.Add(nameof(repoRootPaths), repoRootPaths);

            var reg = MyBorder.Registration;
            reg.SetSettingsDict(settingsDict);
            reg.SetFileService(fileService);
            reg.Registrations();
            
            return settingsDict;
        }

        public List<object> GetRepoSearchPaths()
        {
            var guidFolderName = "18296f12-0706-43e1-9bd4-1b40154ec22e";
            var folderPath1 = fileService.Path.FindFolder(guidFolderName, ".", "3(2,2)");
            var folderPath2 = fileService.Path.MoveDirectoriesUp(folderPath1,1);

            var s1 = Directory.Exists(folderPath2);
            var tmp = Directory.GetDirectories(folderPath2);
            var tmp3 = tmp.Where(x => Guid.TryParse(Path.GetFileName(x), out var tmp2));
            var repoSearchPaths = tmp3.Select(x => (object)x).ToList();
            return repoSearchPaths;
        }

        public void SetConfigService(IConfigService configService)
        {
            this.configService = configService;
        }
    }
}