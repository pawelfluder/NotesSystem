using BlazorUniSystemCore.Components;
using BlazorUniSystemCore.Registrations;

namespace BlazorNotesSystem;

public class AppFasade
{
    private string _rootPath;
    public WebApplication App { get; private set; }
    public WebApplicationBuilder Builder { get; private set; }
    public UniSystemCoreContainer Container { get; private set; }
    public AppFasade()
    {
        InitBuilder();
    }
    
    public void Start()
    {
        try
        {
            InitApp();
            App.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas uruchamiania aplikacji: {ex.Message}");
        }
    }

    private void InitApp()
    {
        Builder = WebApplication.CreateBuilder();

// Add services to the container.
        Builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        App = Builder.Build();

// Configure the HTTP request pipeline.
        if (!App.Environment.IsDevelopment())
        {
            App.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            App.UseHsts();
        }

        App.UseHttpsRedirection();

        App.UseStaticFiles();
        App.UseAntiforgery();

        App.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        App.Run();
    }

    private void InitBuilder()
    {
        Builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            //ContentRootPath = _rootPath,
            //WebRootPath = _rootPath
        });
        
#if DEBUG
    Builder.Environment.EnvironmentName = "Development";
    Builder.WebHost.UseUrls("http://127.0.0.1:5605");
#else
    // this should be set in environment variable
    // ENV ASPNETCORE_HTTP_PORTS=5001
    // EXPOSE 5001
#endif
        
        Container = new UniSystemCoreContainer(
            Builder.Services);
        Builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        
        // Builder.Services.AddSyncfusionBlazor();
        // Builder.Services.AddRazorPages();
        // Builder.Services.AddServerSideBlazor();
        Builder.Services.AddHttpClient();
        
        Builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                corsBuilder =>
                {
                    corsBuilder 
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins(
                            "https://blazor.net", "http://blazor.net",
                            "https://www.facebook.com")
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .Build();
                });
        });
        //Builder.Services.AddFluentUIComponents();
    }
}