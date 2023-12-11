using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using GoogleDriveCoreApp;
using SharpGoogleDriveProg.AAPublic;

namespace SharpGoogleDriveProg.Service
{
    public class GoogleDriveService : IGoogleDriveService
    {
        private DriveService service;
        private bool isInitialized;
        private DriveWorker worker;

        // settings
        private string applicationName;
        private List<string> scopes;
        private string clientId;
        private string clientSecret;

        public DriveWorker Worker
        {
            get
            {
                if (!isInitialized)
                {
                    Initialize(clientId, clientSecret);
                    isInitialized = true;
                }
                return worker;
            }
        }

        public GoogleDriveService()
        {
        }

        public GoogleDriveService(
            Dictionary<string, object> settingDict)
        {
            ApplySettings(settingDict);
        }

        private void ApplySettings(Dictionary<string, object> settingDict)
        {
            var s3 = settingDict.TryGetValue("googleClientId", out var clientId);
            var s4 = settingDict.TryGetValue("googleClientSecret", out var clientSecret);
            if (s3) { this.clientId = clientId.ToString(); }
            if (s4) { this.clientSecret = clientSecret.ToString(); }

            applicationName = "gamestatistics";

            this.scopes = new List<string>
            { 
                DriveService.ScopeConstants.Drive,
                //DriveService.ScopeConstants.DriveAppdata,
                DriveService.ScopeConstants.DriveFile,
                //DriveService.ScopeConstants.DriveMetadata,
                //DriveService.ScopeConstants.DriveScripts,
            };

            //string[] Scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile };
        }

        public void OverrideSettings(Dictionary<string, object> settingDict)
        {
            ApplySettings(settingDict);
        }

        public void Initialize(string clientId, string clientSecret)
        {
            var initializer = GetInitilizer(clientId, clientSecret);
            var service = new DriveService(initializer);
            worker = new DriveWorker(this, service);
        }

        public BaseClientService.Initializer GetInitilizer(string clientId, string clientSecret)
        {
            var secrets = new ClientSecrets();
            secrets.ClientId = clientId;
            secrets.ClientSecret = clientSecret;

            var credentialAuthorization = GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets,
                this.scopes,
                "user",
                CancellationToken.None);

            var initializer = new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentialAuthorization.Result,
                ApplicationName = applicationName,
            };

            return initializer;
        }

        //public List<(string Name, string Id)> GetFilesRequest(string query)
        //{
        //    var files = Worker.GetFilesRequest(query);
        //    return files.Select(x => (x.Name, x.Id)).ToList();
        //}

        //public List<(string Name, string Id)> GetFilesRequest(string query)
        //{
        //    var files = worker.GetFilesRequest(query);
        //    return files.Select(x => (x.Name, x.Id)).ToList();
        //}

        //public (string Name, string Id) GetFolderByNameAndId(string name, string id)
        //{
        //    var googleFile = Worker.GetFolderByNameAndId(name, id);
        //    return (googleFile.Name, googleFile.Id);
        //}

        //public List<(string Name, string Id)> GetAllMp3Files()
        //{
        //    var items = Worker.GetFilesRequest($"fileExtension='mp3'");
        //    return items.Select(x => (x.Name, x.Id)).ToList();
        //}

        //public (string, string) UploadTempPhotoFile(Stream fileStream)
        //{
        //    var result = Worker.UploadTempPhotoFile(fileStream);
        //    return result;
        //}
    }
}
