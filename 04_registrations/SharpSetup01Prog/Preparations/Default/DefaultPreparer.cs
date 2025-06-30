using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpConfigProg;
using SharpConfigProg.AAPublic;
using SharpContainerProg.AAPublic;
using SharpContainerProg.Containers;
using SharpFileServiceProg.AAPublic;
using SharpIdentityProg.AAPublic;
using SharpIdentityProg.Data;
using SharpIdentityProg.Models;
using SharpIdentityProg.Services;
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
        
        var provider = new IdentityDbConnectionProvider();
        AppFasade.WebAppBuilder.Services.AddSingleton<IIdentityDbConnectionProvider>(
            sp => provider);
        // AppFasade.WebAppActionsList.Add(x => 
        //     OutBorder03.UseIdentity(x));

        // AppFasade.WebAppBuilder.Services.AddControllers();
        AppFasade.WebAppBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            string connStr = provider.GetConnStr();
            options.UseSqlite(connStr);
        });
        AppFasade.WebAppBuilder.Services.AddAuthorization();
        AppFasade.WebAppBuilder.Services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        AppFasade.WebAppActionsList.Add(x => x.MapIdentityApi<ApplicationUser>());
        AppFasade.WebAppActionsList.Add(x => x.UseAuthorization());
        // AppFasade.WebAppActionsList.Add(x => x.MapControllers());

        AddEndpoints();
        SeedData2();
    }

    private void SeedData2()
    {
        var _dbContext = AppFasade.Container.Resolve<ApplicationDbContext>();
            
        // ApplicationDbContext _dbContext = _serviceProvider
        //     .GetRequiredService<ApplicationDbContext>();
        List<string> pending = _dbContext.Database
            .GetPendingMigrations()
            .ToList();
        if (pending.Any())
        {
            _dbContext.Database.Migrate();
        }

        AppFasade.WebAppActionsList.Add(async x =>
        {
            using (var scope = x.Services.CreateScope())
            {
                await SeedData.InitializeAsync(scope.ServiceProvider);
            }
        });
    }

    public void AddEndpoints()
        {
            AppFasade.WebAppActionsList.Add(x =>
                 x.MapGet("/api/account", (ClaimsPrincipal user) =>
            {
                if (!user.Identity?.IsAuthenticated ?? true)
                    return Results.Ok("You are NOT authenticated");

                return Results.Ok("You are authenticated");
            }));

            AppFasade.WebAppActionsList.Add(x =>
            x.MapGet("/api/account/Profile", async (
                    UserManager<ApplicationUser> userManager,
                    ClaimsPrincipal user) =>
                {
                    var currentUser = await userManager.GetUserAsync(user);
                    if (currentUser == null)
                        return Results.BadRequest();

                    var userRoles = await userManager.GetRolesAsync(currentUser);

                    var userProfile = new UserProfile
                    {
                        Id = currentUser.Id,
                        Name = currentUser.UserName ?? "",
                        Email = currentUser.Email ?? "",
                        PhoneNumber = currentUser.PhoneNumber ?? "",
                        FirstName = currentUser.FirstName,
                        LastName = currentUser.LastName,
                        Address = currentUser.Address,
                        CreatedAt = currentUser.CreatedAt,
                        Role = string.Join(",", userRoles)
                    };

                    return Results.Ok(userProfile);
                })
                .RequireAuthorization()
                .WithName("GetUserProfile")
                .WithTags("Account"));

            AppFasade.WebAppActionsList.Add(x =>
            x.MapPost("/signup", async (
                UserManager<ApplicationUser> userManager,
                [FromBody] RegisterDto registerDto
            ) =>
            {
                var user = new ApplicationUser
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNumber,
                    Address = registerDto.Address ?? "",
                    CreatedAt = DateTime.Now
                };

                var result = await userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors
                        .GroupBy(e => e.Code)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());

                    return Results.BadRequest(new ValidationProblemDetails(errors));
                }

                await userManager.AddToRoleAsync(user, "client");
                return Results.Ok();
            }));

            AppFasade.WebAppActionsList.Add(x =>
            x.MapGet("/api/account/AdminRoute", () =>
            {
                return Results.Ok("Hello Admin");
            })
            .RequireAuthorization(new AuthorizeAttribute { Roles = "admin" }));

            AppFasade.WebAppActionsList.Add(x =>
            x.MapGet("/api/account/ClientRoute", () =>
            {
                return Results.Ok("Hello Client");
            })
            .RequireAuthorization(new AuthorizeAttribute { Roles = "client" }));

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
        //AppFasade.WebAppBuilder.Services.AddControllers();
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

    // public List<object> GetRepoSearchPaths(
    //     string settingsFolderPath)
    // {
    //     return new List<object>
    //     {
    //         settingsFolderPath
    //     };
    // }

    public void SetConfigService(
        IConfigService configService)
    {
        _configService = configService;
    }
}
