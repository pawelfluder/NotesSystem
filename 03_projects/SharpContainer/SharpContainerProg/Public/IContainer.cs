namespace SharpContainerProg.Public
{
    public interface IContainer
    {
        bool IsRegistered<T>();
        IContainer RegisterSingleton<T>(params object[] injectionMember);
        IContainer RegisterType<T>(params object[] injectionMember);
        T Resolve<T>();
        object Resolve(Type type);
    }
}