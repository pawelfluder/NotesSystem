using Microsoft.AspNetCore.Builder;
using SharpContainerProg.AAPublic;

namespace SharpConfigProg;

public class AppFasade : IAppFasade
{
    private string _rootPath;
    public WebApplicationBuilder Builder { get; private set; }
    public WebApplication App { get; private set; }
    public IContainer4 Container { get; private set; }
    
    public List<Action<WebApplication>> WebAppActionsList { get; }
    
    public AppFasade()
    {
        WebAppActionsList = new();
        InitBuilder();
    }
    
    public void Run()
    {
        try
        {
            InitApp();
            App.Run();
        }
        catch
        {
            Console.WriteLine("Error during start of application run");
        }
    }

    private void InitApp()
    {
        App = Builder.Build();
        ApplyAllActions();
    }
    
    private void ApplyAllActions()
    {
        foreach (var action in WebAppActionsList)
        {
            action.Invoke(App);
        }
    }

    private void InitBuilder()
    {
        Builder = WebApplication.CreateBuilder();
    }
}
