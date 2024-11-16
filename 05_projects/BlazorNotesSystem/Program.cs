using BlazorInterAutoProj.Registrations;
using BlazorNotesSystem.Client.Pages;
using BlazorNotesSystem.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpArgsManagerProj.AAPublic;
using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SharpRepoBackendProg.Service;
using OutBorder01 = SharpSetup21ProgPrivate.AAPublic.OutBorder;

OutBorder01.GetPreparer("PrivateNotesPreparer").Prepare();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

IBackendService backend = MyBorder.Container.Resolve<IBackendService>();
builder.Services.AddSingleton<IBackendService>(backend);

IFileService fileService = MyBorder.Container.Resolve<IFileService>();
builder.Services.AddSingleton<IFileService>(fileService);

IOperationsService operationsService = MyBorder.Container.Resolve<IOperationsService>();
builder.Services.AddSingleton<IOperationsService>(operationsService);

IArgsManagerService argsManager = MyBorder.Container.Resolve<IArgsManagerService>();
builder.Services.AddSingleton<IArgsManagerService>(argsManager);

builder.Services.AddSpeechSynthesisServices();


builder.Services.AddCors(options =>
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

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.Run();
