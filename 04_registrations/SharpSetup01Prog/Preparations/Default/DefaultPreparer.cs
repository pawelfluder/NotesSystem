using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpConfigProg;
using SharpConfigProg.AAPublic;
using SharpContainerProg.AAPublic;
using SharpContainerProg.Containers;
using SharpFileServiceProg.AAPublic;
using SharpIdentityProg.AAPublic;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SharpSetup01Prog.Models;
using OutBorder01 = SharpFileServiceProg.AAPublic.OutBorder;
using OutBorder02 = SharpOperationsProg.AAPublic.OutBorder;
using OutBorder03 = SharpIdentityProg.AAPublic.OutBorder;

namespace SharpSetup01Prog.Preparations.Default;
internal class DefaultPreparer : IPreparer
{
    private IGoogleCredentialWorker _credentials;
    private Dictionary<string, object> _settingsDict = new();
    private IOperationsService _operationsService;
    private IConfigService _configService;
    private bool _isPreparationDone;
    private IFileService _fileService;

    public IAppFasade AppFasade { get; private set; }

    public Dictionary<string, object> Prepare()
    {
        if (_isPreparationDone) return _settingsDict;

        AppFasade = new AppFasade();
        ContainerService.SetOutContainer(AppFasade.Container);
        
        BuilderActions();
        WebAppActions();
        
        // IDENTITY
        AddIdentity();
        
        _fileService = OutBorder01.FileService();
        _operationsService = OutBorder02.OperationsService(_fileService);
        _credentials = _operationsService.Credentials;
        
        PrepareSettings();
        DefaultRegistration reg = new();
        reg.SettingsDict = _settingsDict;
        reg.FileService = _fileService;
        reg.OperationsService = _operationsService;
        reg.Registrations();

        _isPreparationDone = true;
        return _settingsDict;
    }

    private void AddIdentity()
    {
        //AppFasade.WebAppBuilder.Services.AddHttpContextAccessor();
        OutBorder03.AddIdentity(AppFasade.WebAppBuilder);
        AppFasade.WebAppBuilder.Services.AddSingleton<IIdentityDbConnectionProvider>(
            sp => new IdentityDbConnectionProvider());
        AppFasade.WebAppActionsList.Add(x => 
            OutBorder03.UseIdentity(x));
    }

    private void WebAppActions()
    {
        AppFasade.WebAppActionsList.Add(x =>
        {
            if (x.Environment.IsDevelopment())
            {
                x.UseSwagger();
                x.UseSwaggerUI();
            }
        });
        
        AppFasade.WebAppActionsList.Add(x =>
            x.UseHttpsRedirection());
        AppFasade.WebAppActionsList.Add(x => 
            x.MapControllers());
        AppFasade.WebAppActionsList.Add(x => 
            x.UseCors());
        
        AppFasade.WebAppActionsList.Add(x => 
            x.Urls.Add("http://localhost:6602"));
        AppFasade.WebAppActionsList.Add(x => 
            x.Urls.Add("https://localhost:6603"));
    }

    private void BuilderActions()
    {
        AppFasade.WebAppBuilder.Services.AddEndpointsApiExplorer();
        AppFasade.WebAppBuilder.Services.AddSwaggerGen();
        AppFasade.WebAppBuilder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins("https://localhost:7262")
                    .WithOrigins("http://localhost:5156")
                    
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        AppFasade.WebAppBuilder.Services.AddControllers();
    }

    private void PrepareSettings()
    {
        PrintStartPaths();
        ConfigBase config = new ReleaseLocalConfig(
            _operationsService,
            _credentials);
        config.AddRangeForSettings(_settingsDict);
    }

    private void PrintStartPaths()
    {
        string currentDirectoryPath = Directory.GetCurrentDirectory();
        Console.WriteLine("CurrentDirectoryPath: " + currentDirectoryPath);
        string currentPath2 = Environment.CurrentDirectory;
        Console.WriteLine("EnvCurrentDirectoryPath: " + currentPath2);
        
        // system directory
        string systemPath = Environment.SystemDirectory;
        Console.WriteLine("systemPath: " + systemPath);
    }

    public List<object> GetRepoSearchPaths(
        string settingsFolderPath)
    {
        return new List<object>
        {
            settingsFolderPath
        };
    }

    public void SetConfigService(
        IConfigService configService)
    {
        _configService = configService;
    }
}
