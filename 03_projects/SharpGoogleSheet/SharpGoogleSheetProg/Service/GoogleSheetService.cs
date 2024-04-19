using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using SharpGoogleSheetProg.AAPublic;
using SharpGoogleSheetProg.Names;

namespace SharpGoogleSheetProg.Service
{
    public class GoogleSheetService : IGoogleSheetService
    {
        // workers
        private SheetsService service;
        private SheetWorker worker;
        private bool isSheetInit;

        // settings
        private Dictionary<string, object> settings;
        private List<string> scopes;
        private string clientId;
        private string clientSecret;
        private string applicationName;
        private string user;

        public GoogleSheetService()
        {
        }

        public GoogleSheetService(
            Dictionary<string, object> settingDict)
        {
            ReWriteSettings(settingDict);
        }

        public SheetWorker Worker
        {
            get
            {
                if (!isSheetInit)
                {
                    SheetInit();
                    isSheetInit = true;
                }
                return worker;
            }
        }

        public void Initialize()
        {
            SheetInit();
        }

        public void OverrideSettings(Dictionary<string, object> settingDict)
        {
            ReWriteSettings(settingDict);
            ApplySettings();
        }

        private void ReWriteSettings(Dictionary<string, object> inputDict)
        {
            this.settings ??= new Dictionary<string, object>();
            TryAdd(inputDict, settings, VarNames.GoogleClientId);
            TryAdd(inputDict, settings, VarNames.GoogleClientSecret);
            TryAdd(inputDict, settings, VarNames.GoogleApplicationName);
            TryAdd(inputDict, settings, VarNames.GoogleUserName);
        }

        private void ApplySettings()
        {
            this.scopes ??= new List<string>();
            this.clientId = settings[VarNames.GoogleClientId].ToString();
            this.clientSecret = settings[VarNames.GoogleClientSecret].ToString();
            this.applicationName = settings[VarNames.GoogleApplicationName].ToString();
            this.user = settings[VarNames.GoogleUserName].ToString();
            
            this.scopes.Add(SheetsService.ScopeConstants.Spreadsheets);
            //scopes.Add(SheetsService.ScopeConstants.Drive);
        }

        private void SheetInit()
        {
            ApplySettings();
            var initializer = GetInitilizer(clientId, clientSecret);
            service = new SheetsService(initializer);
            worker = new SheetWorker(this, service);
        }

        public BaseClientService.Initializer GetInitilizer(
            string clientId,
            string clientSecret)
        {
            var secrets = new ClientSecrets();
            secrets.ClientId = clientId;
            secrets.ClientSecret = clientSecret;

            var credentialAuthorization = GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets,
                this.scopes,
                this.user,
                CancellationToken.None);

            var initializer = new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentialAuthorization.Result,
                ApplicationName = applicationName,
            };

            return initializer;
        }

        private bool TryAdd(
            Dictionary<string, object> inputDict,
            Dictionary<string, object> outputDict,
            string keyName)
        {
            var success1 = inputDict.TryGetValue(keyName, out var valueObj);
            var success2 = false;
            if (success1)
            {
                success2 = outputDict.TryAdd(keyName, valueObj);
            }

            if (success2)
            {
                return true;
            }

            return false;
        }
    }
}