using BlazorInterAutoProj.Registrations;
using BlazorNotesSystem.Client.Pages;
using BlazorNotesSystem.Components;
using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;

using OutBorder01 = SharpSetup21ProgPrivate.AAPublic.OutBorder;

OutBorder01.GetPreparer("PrivateNotesPreparer").Prepare();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var backend = new BackendService();
var fileService = MyBorder.Container.Resolve<IFileService>();
builder.Services.AddSingleton<BackendService>(backend);
builder.Services.AddSingleton<IFileService>(fileService);

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
