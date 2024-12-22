using BlazorNotesSystem.Client.Pages;
using BlazorNotesSystem.Components;
using BlazorNotesSystem.Registrations;
using Microsoft.Fast.Components.FluentUI;

namespace BlazorNotesSystem;

public class AppFasade
{
    private string _gg;
    public WebApplication App { get; private set; }
    public WebApplicationBuilder Builder { get; private set; }
    public UniSystemCoreContainer Container { get; private set; }
    public AppFasade()
    {
        InitBuilder();
    }
    public void Start()
    {
        InitApp();
        App.Start();
    }

    private void InitApp()
    {
        App = Builder.Build();
        Container.SetServiceProvider(App.Services);
        if (App.Environment.IsDevelopment())
        {
            App.UseWebAssemblyDebugging();
        }
        else
        {
            App.UseExceptionHandler("/Error", createScopeForErrors: true);
            App.UseHsts();
        }

        App.UseHttpsRedirection();
        App.UseStaticFiles(_gg);
        App.UseAntiforgery();
        App.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Counter).Assembly);
    }

    private void InitBuilder()
    {
        _gg = "/Users/pawelfluder/03_synch/01_files_programming/03_github/NotesSystem/05_projects/BlazorUniSystemCore/wwwroot";

        Builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ContentRootPath = _gg,
            WebRootPath = _gg
        });
        
#if DEBUG
    Builder.Environment.EnvironmentName = "Development";
#else
    builder.Environment.EnvironmentName = "Production";
#endif
        
        Container = new UniSystemCoreContainer(
            Builder.Services);
        Builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();
        
        Builder.WebHost.UseUrls("http://127.0.0.1:5000");
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
        Builder.Services.AddFluentUIComponents();
    }
}