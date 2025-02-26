using Microsoft.Extensions.DependencyInjection;

namespace SharpContainerProg.AAPublic;
public interface IContainer4
{
    IServiceCollection ServiceRegister { get; }
    IServiceProvider ServiceProvider { get; }

    // void RegisterSingleton<RegT>(
    //     RegT obj)
    //     where RegT : class;

    void RegisterByFunc<RegT>(
        Func<RegT> func,
        int type = 0,
        Action endAction = null)
        where RegT : class;
    
    void RegisterByFunc<P1, RegT>(
        Func<P1, RegT> regTfunc,
        Func<P1> p1Tfunc,
        int type = 0,
        Action endAction = null)
        where RegT : class
        where P1 : class;

    void RegisterByFunc<P1, P2, RegT>(
        Func<P1, P2, RegT> regTfunc,
        Func<P1> p1Tfunc,
        Func<P2> p2Tfunc,
        int type = 0,
        Action endAction = null)
        where RegT : class
        where P1 : class;

    T Resolve<T>();

    object? Resolve(
        Type type);

    bool IsRegistered<T>();
}
