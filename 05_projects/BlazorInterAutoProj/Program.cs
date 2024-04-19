using BlazorInterAutoProj.Components;
using BlazorInterAutoProj.Registrations;
using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Service;
using OutBorder01 = SharpSetup21ProgPrivate.AAPublic.OutBorder;

OutBorder01.GetPreparer("PrivateNotesPreparer").Prepare();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
    app.UseHsts();
}

app.UseHttpsRedirection();

var tmp1 = app.Services.GetService<BackendService>();
var tmp2 = app.Services.GetService<IFileService>();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorInterAutoProj.Client._Imports).Assembly);

app.Run();
