using BlazorNotesSystem.Client.Pages;
using BlazorNotesSystem.Components;
using BlazorNotesSystem.Registrations;

namespace BlazorNotesSystem;

public class AppFasade
{
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
        App.UseStaticFiles();
        App.UseAntiforgery();
        App.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Counter).Assembly);
    }

    private void InitBuilder()
    {
        Builder = WebApplication.CreateBuilder();
        Container = new UniSystemCoreContainer(
            Builder.Services);
        Builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();
        
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
    }
}