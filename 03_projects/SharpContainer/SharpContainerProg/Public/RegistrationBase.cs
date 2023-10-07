using SharpContainerProg.Repetition;
using Unity.Injection;

namespace SharpContainerProg.Public
{
    public abstract class RegistrationBase
    {
        private static IContainer container;
        private bool registrationStarted;

        public RegistrationBase()
        {
            if (container == null)
            {
                container = new Container();
            }
        }

        public IContainer Start()
        {
            if (!registrationStarted)
            {
                registrationStarted = true;
                Registrations();
            }

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

        public void RegisterByFunc<R, T1, T2>(Func<T1, T2, R> func, T1 t1, T2 t2)
        {
            container.RegisterType<R>(new InjectionFactory(c =>
            {
                return func.Invoke(t1, t2);
            }));
        }
    }
}
