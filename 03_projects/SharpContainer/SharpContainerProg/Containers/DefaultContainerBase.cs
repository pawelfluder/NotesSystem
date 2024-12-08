using Microsoft.Extensions.DependencyInjection;
using SharpContainerProg.AAPublic;

namespace SharpContainerProg.Containers;

public class DefaultContainerBase : IContainer4
{
    private bool _isBuildDone;
    public IServiceCollection ServiceRegister { get; protected set; }
    public IServiceProvider ServiceProvider { get; protected set; }

    public void RegisterSingleton<RegT>(
        RegT obj)
            where RegT : class
    {
        ServiceRegister.AddSingleton<RegT>(sp => 
            obj);
    }
    
    public void RegisterByFunc<T>(
        Func<T> func,
        int type = 0)
            where T : class
    {
        if (type == 0)
        {
            ServiceRegister.AddSingleton<T>(sp => 
                func.Invoke());
        }
        if (type == 1)
        {
            ServiceRegister.AddTransient<T>(sp => 
                func.Invoke());
        }
    }

    public T Resolve<T>()
    {
        DoIfNotYetBuild();
        object? service = ServiceProvider.GetService(typeof(T));
        return (T)service!;
    }

    public object? Resolve(
        Type type)
    {
        DoIfNotYetBuild();
        object? service = ServiceProvider.GetService(type);
        return service;
    }
    
    public bool IsRegistered<T>()
    {
        bool isRegistered = ServiceRegister
            .Any(sd => sd.ServiceType == typeof(T));
        return isRegistered;
    }
    
    private void DoIfNotYetBuild()
    {
        if (_isBuildDone)
        {
            return;
        }

        ServiceProvider = ServiceRegister.BuildServiceProvider();
    }
}
