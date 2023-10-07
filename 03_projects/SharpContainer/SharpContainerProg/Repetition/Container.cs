using SharpContainerProg.Public;
using Unity;
using Unity.Injection;

namespace SharpContainerProg.Repetition
{
    internal class Container : IContainer
    {
        private UnityContainer unity = new UnityContainer();

        public bool IsRegistered<T>()
        {
            var result = UnityContainerExtensions.IsRegistered(unity, typeof(T));
            return result;
        }

        public T Resolve<T>()
        {
            var result = UnityContainerExtensions.Resolve<T>(unity);
            return result;
        }

        public IContainer RegisterSingleton<T>(params object[] injectionMember)
        {
            var tmp = injectionMember.Select(x => (InjectionMember)x).ToArray();
            var result = UnityContainerExtensions.RegisterSingleton<T>(unity, tmp);
            return this;
        }

        public IContainer RegisterType<T>(params object[] injectionMember)
        {
            var tmp = injectionMember.Select(x => (InjectionMember)x).ToArray();
            var result = UnityContainerExtensions.RegisterType<T>(unity, tmp);
            return this;
        }
    }
}
