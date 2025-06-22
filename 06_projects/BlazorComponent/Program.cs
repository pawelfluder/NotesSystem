using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorComponent;
using BlazorComponent.Workers;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var url = "http://localhost:5000";
builder.Services.AddScoped<BackendAdapter>(sp => 
    new BackendAdapter(url));

builder.Services.AddScoped(sp => 
    new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    });

await builder.Build().RunAsync();
