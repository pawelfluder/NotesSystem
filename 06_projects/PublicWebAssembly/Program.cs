using BackendAdapters.Workers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using PublicWebAssembly;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => 
{
    HttpClient client = sp.GetRequiredService<HttpClient>();
    return new BackendAdapter(client);
});

builder.Services.AddScoped(sp =>
{
    BackendAdapter backend = sp.GetRequiredService<BackendAdapter>();
    return new RepoAdapter(backend);
});


builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();