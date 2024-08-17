using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Services;
using SharpGoogleDocsProg.AAPublic;
using SharpGoogleDocsProg.Composits;
using SharpGoogleDocsProg.Names;
using SharpGoogleDocsProg.Worker;

namespace SharpGoogleDocsProg.Service
{
    internal class GoogleDocsService : IGoogleDocsService
    {
        // composits
        private DocumentComposite documentComposite;
        private RequestsCoposite requestsComposite;
        private StackCoposite stackComposite;
        private ExecuteComposite executeComposite;
        
        // init
        private bool isSettingsInit;
        private bool isDocumentInit;
        private bool isRequestsInit;
        private bool isStackInit;
        private bool isExecuteInit;

        // settings
        private Dictionary<string, object> _settings;
        private List<string> scopes;
        private string clientId;
        private string clientSecret;
        private string applicationName;
        private string user;
        
        public DocumentComposite Document { get
        {
            if (!isDocumentInit){ DocumentInit(); isDocumentInit = true; }
            return documentComposite;
        }}
        public StackCoposite Stack { get
        {
            if (!isStackInit){ StackInit(); isStackInit = true; }
            return stackComposite;
        }}

        public ExecuteComposite Execute { get
        {
            if (!isExecuteInit){ ExecuteInit(); isStackInit = true; }
            return executeComposite;
        }}
        
        // public GoogleDocsService()
        // {
        // }
        
        public GoogleDocsService(
            Dictionary<string, object> settingDict)
        {
            ReWriteSettings(settingDict);
        }
        
        // settings
        public void OverrideSettings(Dictionary<string, object> settingDict)
        {
            ReWriteSettings(settingDict);
            ApplySettings();
        }
        private void ReWriteSettings(Dictionary<string, object> inputDict)
        {
            _settings = new Dictionary<string, object>();
            TryAdd(inputDict, _settings, VarNames.GoogleClientId);
            TryAdd(inputDict, _settings, VarNames.GoogleClientSecret);
            TryAdd(inputDict, _settings, VarNames.GoogleApplicationName);
            TryAdd(inputDict, _settings, VarNames.GoogleUserName);
        }
        private void ApplySettings(bool rewrite = false)
        {
            if (!isSettingsInit || rewrite)
            {
                this.scopes ??= new List<string>();
                this.scopes.Add(DocsService.ScopeConstants.Documents);
                this.clientId = _settings[VarNames.GoogleClientId].ToString();
                this.clientSecret = _settings[VarNames.GoogleClientSecret].ToString();
                this.applicationName = _settings[VarNames.GoogleApplicationName].ToString();
                this.user = _settings[VarNames.GoogleUserName].ToString();
                isSettingsInit = true;
            }
        }
        // http client initializer
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
        // init
        public void Initialize()
        {
            StackInit();
        }
        private void DocumentInit()
        {
            ApplySettings();
            
            requestsComposite = new RequestsCoposite(service);
            stackComposite = new StackCoposite(service);
        }
        private void ExecuteInit()
        {
            throw new NotImplementedException();
        }
        private void RequestInit()
        {
            throw new NotImplementedException();
        }
        private void StackInit()
        {
            ApplySettings();
            var initializer = GetInitilizer(clientId, clientSecret);
            var service = new DocsService(initializer);
            requestsComposite = new RequestsCoposite(service);
            stackComposite = new StackCoposite(service);
        }
        private void DocsSericeInit()
        {
            var initializer = GetInitilizer(clientId, clientSecret);
            var service = new DocsService(initializer);
        }
    }
}