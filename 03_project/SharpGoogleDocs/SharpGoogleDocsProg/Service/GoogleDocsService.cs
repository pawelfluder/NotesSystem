using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Services;

namespace GoogleDocsServiceProj.Service
{
    public class GoogleDocsService
    {
        private readonly IEnumerable<string> Scopes;

        public DocsWorker Worker { get; private set; }
        public StackWorker StackWkr { get; private set; }

        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string applicationName;

        public GoogleDocsService(
            string clientId,
            string clientSecret,
            string applicationName,
            IEnumerable<string> Scopes)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.applicationName = applicationName;
            Initialize();
        }

        public void Initialize()
        {
            var initializer = GetInitilizer(clientId, clientSecret);
            var service = new DocsService(initializer);
            Worker = new DocsWorker(service);
            StackWkr = new StackWorker(service);
        }

        public BaseClientService.Initializer GetInitilizer(string clientId, string clientSecret)
        {
            var secrets = new ClientSecrets();
            secrets.ClientId = clientId;
            secrets.ClientSecret = clientSecret;

            var credentialAuthorization = GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets,
                Scopes,
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
