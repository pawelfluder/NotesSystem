using Unity;
using Unity.Injection;

namespace SharpRepoBackendProg.Repetition
{
    internal abstract class RegistrationBase
    {
        public UnityContainer container;

        public RegistrationBase()
        {
            container = new UnityContainer();
            Registrations();
        }

        public UnityContainer Start()
        {
            return container;
        }

        protected abstract void Registrations();

        public void RegisterByFunc<R>(Func<R> func)
        {
            var factory = new InjectionFactory(c =>
            {
                return func.Invoke();
            });
            container.RegisterSingleton<R>(factory);
        }

        public void RegisterByFunc<R, T1>(Func<T1, R> func, T1 t1)
        {
            container.RegisterSingleton<R>(new InjectionFactory(c =>
            {
                return func.Invoke(t1);
            }));
        }

        public void RegisterSingleton<R, T1, T2>(Func<T1, T2, R> func, T1 t1, T2 t2)
        {
            container.RegisterType<R>(new InjectionFactory(c =>
            {
                return func.Invoke(t1, t2);
            }));
        }
    }
}
