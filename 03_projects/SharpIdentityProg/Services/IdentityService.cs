using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharpIdentityProg.AAPublic;
using SharpIdentityProg.Data;
using SharpIdentityProg.Registrations;

namespace SharpIdentityProg.Services;

public class IdentityService : IIdentityService
{
    private ApplicationDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    private IApiRegister _apiRegister;
    private bool _isInit;

    public IdentityService()
    {
        ApiRegister = MyBorder.MyContainer.Resolve<IApiRegister>();
        _serviceProvider = MyBorder.MyContainer.ServiceProvider;
    }

    public IApiRegister ApiRegister
    {
        get
        {
            Init().GetAwaiter().GetResult();
            return _apiRegister;
        }
        set => _apiRegister = value;
    }
    
    private async Task Init()
    {
        if (_isInit) return;
        
        _dbContext = MyBorder.OutContainer.Resolve<ApplicationDbContext>();
            
        // ApplicationDbContext _dbContext = _serviceProvider
        //     .GetRequiredService<ApplicationDbContext>();
        List<string> pending = _dbContext.Database
            .GetPendingMigrations()
            .ToList();
        if (pending.Any())
        {
            _dbContext.Database.Migrate();
        }
            
        using (var scope = _serviceProvider.CreateScope())
        {
            await SeedData.InitializeAsync(scope.ServiceProvider);
        }
        
        _isInit = true;
    }
}
