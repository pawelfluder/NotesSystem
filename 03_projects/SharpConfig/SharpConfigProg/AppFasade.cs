using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SharpContainerProg.AAPublic;
using SharpContainerProg.Containers;
using SharpIdentityProg.Data;
using SharpIdentityProg.Models;

namespace SharpConfigProg;

public class AppFasade : IAppFasade
{
    // Container
    public IContainer4 Container { get; private set; }
    
    // Building services
    public WebApplicationBuilder WebAppBuilder { get; private set; }
    public IServiceCollection ServicesCollection { get; private set; }
    
    // App services
    public WebApplication WebApp { get; private set; }
    public IServiceProvider ServiceProvider { get; private set; }
    public List<Action<WebApplication>> WebAppActionsList { get; }
    
    public AppFasade()
    {
        WebAppBuilder = WebApplication.CreateBuilder();
        Container = new DefaultContainer(WebAppBuilder.Services);
        ServicesCollection = WebAppBuilder.Services;
        
        WebAppActionsList = new();
    }

    public void Run()
    {
        try
        {
            WebApp = WebAppBuilder.Build();
            ServiceProvider = WebApp.Services;
            ApplyAllActions();
            var gg2 = WebApp.Services.GetHashCode();
            var gg = WebApp.Services.GetRequiredService<IEmailSender<ApplicationUser>>();
            WebApp.Run();
        }
        catch
        {
            Console.WriteLine("Error during start of application run");
        }
    }
    private void ApplyAllActions()
    {
        foreach (var action in WebAppActionsList)
        {
            action.Invoke(WebApp);
        }
    }
}
