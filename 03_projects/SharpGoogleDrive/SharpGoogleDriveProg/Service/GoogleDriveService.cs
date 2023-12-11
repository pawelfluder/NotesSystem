using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
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

            applicationName = "notesSystem";

            this.scopes = new List<string>
            { 
                DriveService.ScopeConstants.Drive,
                //DriveService.ScopeConstants.DriveAppda
                DriveService.ScopeConstants.DriveFile,
                DriveService.ScopeConstants.DriveMetadata,
                DriveService.ScopeConstants.DriveScripts,
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
                "GameStatistics",
                CancellationToken.None);

            var initializer = new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentialAuthorization.Result,
                ApplicationName = applicationName,
            };

            return initializer;
        }
    }
}
