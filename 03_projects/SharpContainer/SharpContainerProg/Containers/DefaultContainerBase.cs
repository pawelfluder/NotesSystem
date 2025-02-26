using Microsoft.Extensions.DependencyInjection;
using SharpContainerProg.AAPublic;

namespace SharpContainerProg.Containers;

public class DefaultContainerBase : IContainer4
{
    private bool _isBuildDone;
    public IServiceCollection ServiceRegister { get; protected set; }

    private IServiceProvider _serviceProvider;
    private bool _isNothingRegistered = true;

    public IServiceProvider ServiceProvider { get; protected set; }

    public virtual void RegisterMocks()
    {
    }

    public void RegisterByFunc<RegT>(
        Func<RegT> func,
        int type = 0,
        Action endAction = null)
            where RegT : class
    {
        if (type == 0)
        {
            ServiceRegister.AddSingleton<RegT>(
                func.Invoke());
        }
        if (type == 1)
        {
            ServiceRegister.AddTransient<RegT>(sp => 
                func.Invoke());
        }
        if (type == 2)
        {
            ServiceRegister.AddScoped<RegT>(sp => 
                func.Invoke());
        }
        
        if (endAction != null)
        {
            endAction.Invoke();
        }
    }
    
    public void RegisterByFunc<P1, RegT>(
        Func<P1, RegT> regTfunc,
        Func<P1> p1Tfunc,
        int type = 0,
        Action endAction = null)
        where RegT : class
        where P1 : class
    {
        if (type == 0)
        {
            ServiceRegister.AddSingleton<RegT>(
                sp => regTfunc.Invoke(
                    p1Tfunc.Invoke()));
        }
        if (type == 1)
        {
            ServiceRegister.AddTransient<RegT>(
                sp => regTfunc.Invoke(
                    p1Tfunc.Invoke()));
        }
        if (type == 2)
        {
            ServiceRegister.AddScoped<RegT>(
                sp => regTfunc.Invoke(
                    p1Tfunc.Invoke()));
        }
        
        if (endAction != null)
        {
            endAction.Invoke();
        }
    }
    
    public void RegisterByFunc<P1, P2, RegT>(
        Func<P1, P2, RegT> regTfunc,
        Func<P1> p1Tfunc,
        Func<P2> p2Tfunc,
        int type = 0,
        Action endAction = null)
        where RegT : class
        where P1 : class
    {
        if (type == 0)
        {
            ServiceRegister.AddSingleton<RegT>(
                sp => regTfunc.Invoke(
                    p1Tfunc.Invoke(),
                    p2Tfunc.Invoke()));
        }
        if (type == 1)
        {
            ServiceRegister.AddTransient<RegT>(
                sp => regTfunc.Invoke(
                    p1Tfunc.Invoke(),
                    p2Tfunc.Invoke()));
        }
        if (type == 2)
        {
            ServiceRegister.AddScoped<RegT>(
                sp => regTfunc.Invoke(
                    p1Tfunc.Invoke(),
                    p2Tfunc.Invoke()));
        }
        
        if (endAction != null)
        {
            endAction.Invoke();
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

        if (ServiceRegister.Count == 0)
        {
            RegisterMocks();
        }
        ServiceProvider = ServiceRegister.BuildServiceProvider();
    }
}
