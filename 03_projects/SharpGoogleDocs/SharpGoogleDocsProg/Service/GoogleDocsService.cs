using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Services;
using SharpGoogleDocsProg.AAPublic;
using SharpGoogleDocsProg.Composits;
using SharpGoogleDocsProg.Names;
using SharpGoogleDocsProg.Worker;

namespace SharpGoogleDocsProg.Service;

internal class GoogleDocsService : IGoogleDocsService
{
    // composits
    private DocumentComposite _documentComposite;
    private RequestsCoposite _requestsComposite;
    private StackCoposite _stackComposite;
    private ExecuteComposite _executeComposite;
    private DocsService _docsService;
        
    // init
    private bool _isSettingsInit;
    private bool _isDocsServiceInit;
    private bool _isDocumentInit;
    private bool _isRequestsInit;
    private bool _isStackInit;
    private bool _isExecuteInit;

    // settings
    private Dictionary<string, object> _settings;
    private List<string> scopes;
    private string clientId;
    private string clientSecret;
    private string applicationName;
    private string user;

    public DocumentComposite Document { get
    {
        if (!_isDocumentInit){ DocumentInit(); _isDocumentInit = true; }
        return _documentComposite;
    }}
    public StackCoposite Stack { get
    {
        if (!_isStackInit){ StackInit(); _isStackInit = true; }
        return _stackComposite;
    }}

    public ExecuteComposite Execute { get
    {
        if (!_isExecuteInit){ ExecuteInit(); _isExecuteInit = true; }
        return _executeComposite;
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
        if (!_isSettingsInit || rewrite)
        {
            this.scopes ??= new List<string>();
            this.scopes.Add(DocsService.ScopeConstants.Documents);
            this.clientId = _settings[VarNames.GoogleClientId].ToString();
            this.clientSecret = _settings[VarNames.GoogleClientSecret].ToString();
            this.applicationName = _settings[VarNames.GoogleApplicationName].ToString();
            this.user = _settings[VarNames.GoogleUserName].ToString();
            _isSettingsInit = true;
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
        if (!_isDocsServiceInit) { DocsServiceInit(); }
        _documentComposite = new DocumentComposite(_docsService);
    }
    private void ExecuteInit()
    {
        if (!_isRequestsInit) { RequestInit(); }
        if (!_isDocumentInit) { DocumentInit(); }
        _executeComposite = new ExecuteComposite(_requestsComposite, _documentComposite);
        _isExecuteInit = true;
    }
    private void RequestInit()
    {
        if (!_isDocsServiceInit) { DocsServiceInit(); }
        if (!_isDocumentInit) { DocumentInit(); }
        _requestsComposite = new RequestsCoposite(_docsService, _documentComposite);
        _isRequestsInit = true;
    }
    private void StackInit()
    {
        if (!_isDocsServiceInit) { DocsServiceInit(); }
        if (!_isDocumentInit) { DocsServiceInit(); }
        _stackComposite = new StackCoposite(_docsService, _documentComposite);
        _isStackInit = true;
    }
    private void DocsServiceInit()
    {
        if (!_isSettingsInit) { ApplySettings(); }
        var initializer = GetInitilizer(clientId, clientSecret);
        _docsService = new DocsService(initializer);
        _isDocsServiceInit = true;
    }
}