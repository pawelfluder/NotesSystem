using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Services;
using SharpGoogleDocsProg.AAPublic;

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
            var s1 = settingDict.TryGetValue("applicationName", out var applicationName);
            var s2 = settingDict.TryGetValue("scopes", out var scopes);
            var s3 = settingDict.TryGetValue("clientId", out var clientId);
            var s4 = settingDict.TryGetValue("clientSecret", out var clientSecret);
            if (s1) { this.applicationName = applicationName.ToString(); }
            if (s2) { this.scopes = applicationName as List<string>; }
            if (s3) { this.applicationName = clientId.ToString(); }
            if (s4) { this.applicationName = clientSecret.ToString(); }
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
                scopes,
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
