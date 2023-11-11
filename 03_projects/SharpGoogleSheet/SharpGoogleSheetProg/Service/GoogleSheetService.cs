using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using SharpGoogleSheetProg.AAPublic;

namespace SharpGoogleSheetProg.Service
{
    public class GoogleSheetService : IGoogleSheetService
    {
        private string[] Scopes = { SheetsService.ScopeConstants.Spreadsheets };
        private SheetsService sheetsService;
        private SheetWorker worker;
        private string applicationName;
        private List<string> scopes;
        private string clientId;
        private string clientSecret;
        private bool isInitialized;
        private List<Request> stack;

        public SheetWorker Worker
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

        public GoogleSheetService()
        {
        }

        public GoogleSheetService(
            Dictionary<string, object> settingDict)
        {
            ApplySettings(settingDict);
        }

        private void ApplySettings(Dictionary<string, object> settingDict)
        {
            var s1 = settingDict.TryGetValue("applicationName", out var applicationName);
            var s2 = settingDict.TryGetValue("scopes", out var scopes);
            var s3 = settingDict.TryGetValue("googleClientId", out var clientId);
            var s4 = settingDict.TryGetValue("googleClientSecret", out var clientSecret);
            if (s1) { this.applicationName = applicationName.ToString(); }
            if (s2) { this.scopes = applicationName as List<string>; }
            if (s3) { this.clientId = clientId.ToString(); }
            if (s4) { this.clientSecret = clientSecret.ToString(); }
        }

        public void OverrideSettings(Dictionary<string, object> settingDict)
        {
            ApplySettings(settingDict);
        }

        public void Initialize(string clientId, string clientSecret)
        {
            string[] Scopes = { SheetsService.Scope.Spreadsheets};
            string ApplicationName = GetType().Name;

            var secrets = new ClientSecrets();
            secrets.ClientId = clientId;
            secrets.ClientSecret = clientSecret;

            var credentialAuthorization = GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets,
                Scopes,
                "user",
                CancellationToken.None);

            // Create Google Sheets API service.
            sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentialAuthorization.Result,
                ApplicationName = applicationName,
            });

            worker = new SheetWorker(this, sheetsService);
        }
    }
}
