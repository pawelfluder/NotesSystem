using BlazorInterAutoProj.Registrations;
using BlazorNotesSystem.Client.Pages;
using BlazorNotesSystem.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpArgsManagerProj.AAPublic;
using SharpFileServiceProg.AAPublic;
using SharpGameSynchProg.Service;
using SharpOperationsProg.AAPublic.Operations;
using SharpRepoBackendProg.Service;
using OutBorder01 = SharpSetup21ProgPrivate.AAPublic.OutBorder;

OutBorder01.GetPreparer("PrivateNotesPreparer").Prepare();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

IBackendService backend = MyBorder.Container.Resolve<IBackendService>();
builder.Services.AddSingleton<IBackendService>(backend);

var fileService = MyBorder.Container.Resolve<IFileService>();
builder.Services.AddSingleton<IFileService>(fileService);

var operationsService = MyBorder.Container.Resolve<IOperationsService>();
builder.Services.AddSingleton<IOperationsService>(operationsService);

var argsManager = MyBorder.Container.Resolve<IArgsManagerService>();
builder.Services.AddSingleton<IArgsManagerService>(argsManager);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins(
                    "http://google.com","https://google.com")
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

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
