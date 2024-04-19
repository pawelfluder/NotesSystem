using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Services;
using SharpGoogleDocsProg.AAPublic;
using SharpGoogleDocsProg.Names;
using SharpGoogleDocsProg.Worker;

namespace GoogleDocsServiceProj.Service
{
    internal class GoogleDocsService : IGoogleDocsService
    {
        // workers
        private StackWorker stackWkr;
        private DocsWorker worker;
        private bool isStackInit;

        // settings
        private Dictionary<string, object> settings;
        private List<string> scopes;
        private string clientId;
        private string clientSecret;
        private string applicationName;
        private string user;

        public GoogleDocsService()
        {
        }

        public GoogleDocsService(
            Dictionary<string, object> settingDict)
        {
            ReWriteSettings(settingDict);
        }

        public StackWorker StackWkr
        {
            get
            {
                if (!isStackInit)
                {
                    StackInit();
                    isStackInit = true;
                }
                return stackWkr;
            }
        }

        public void Initialize()
        {
            StackInit();
        }

        public void OverrideSettings(Dictionary<string, object> settingDict)
        {
            ReWriteSettings(settingDict);
            ApplySettings();
        }

        private void ReWriteSettings(Dictionary<string, object> inputDict)
        {
            this.settings = new Dictionary<string, object>();
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

            this.scopes.Add(DocsService.ScopeConstants.Documents);
        }

        private void StackInit()
        {
            ApplySettings();
            var initializer = GetInitilizer(clientId, clientSecret);
            var service = new DocsService(initializer);
            worker = new DocsWorker(service);
            stackWkr = new StackWorker(service);
        }

        public BaseClientService.Initializer GetInitilizer(string clientId, string clientSecret)
        {
            var secrets = new ClientSecrets();
            secrets.ClientId = clientId;
            secrets.ClientSecret = clientSecret;

            var task = GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets,
                this.scopes,
                user,
                CancellationToken.None);

            task.Wait();

            var initializer = new BaseClientService.Initializer()
            {
                HttpClientInitializer = task.Result,
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