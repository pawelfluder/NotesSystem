using Microsoft.Extensions.DependencyInjection;

namespace SharpContainerProg.AAPublic;
public interface IContainer4
{
    IServiceCollection ServiceRegister { get; }
    IServiceProvider ServiceProvider { get; }

    void RegisterSingleton<RegT>(
        RegT obj)
        where RegT : class;

    void RegisterByFunc<T>(
        Func<T> func,
        int type = 0)
        where T : class;

    T Resolve<T>();

    object? Resolve(
        Type type);

    bool IsRegistered<T>();
}
