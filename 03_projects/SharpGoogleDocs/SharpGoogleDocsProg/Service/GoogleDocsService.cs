using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Services;
using SharpGoogleDocsProg.AAPublic;
using SharpGoogleDocsProg.Worker;

namespace GoogleDocsServiceProj.Service
{
    internal class GoogleDocsService : IGoogleDocsService
    {
        // workers
        private bool isInitialized;
        private DocsWorker worker;
        private StackWorker stackWkr;

        // settings
        private IEnumerable<string> scopes;
        private string clientId;
        private string clientSecret;
        private string applicationName;

        public GoogleDocsService()
        {
        }

        public GoogleDocsService(
            Dictionary<string, object> settingDict)
        {
            ApplySettings(settingDict);
        }

        public StackWorker StackWkr
        {
            get
            {
                if (!isInitialized)
                {
                    Initialize(clientId, clientSecret);
                    isInitialized = true;
                }
                return stackWkr;
            }
        }

        private void ApplySettings(Dictionary<string, object> settingDict)
        {
            var s3 = settingDict.TryGetValue("googleClientId", out var clientId);
            var s4 = settingDict.TryGetValue("googleClientSecret", out var clientSecret);
            this.applicationName = "notesSystem";
            this.scopes = new List<string> { DocsService.ScopeConstants.Documents };
            if (s3) { this.clientId = clientId.ToString(); }
            if (s4) { this.clientSecret = clientSecret.ToString(); }
        }

        public void OverrideSettings(Dictionary<string, object> settingDict)
        {
            ApplySettings(settingDict);
        }

        public void Initialize(string clientId, string clientSecret)
        {
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
    }
}
