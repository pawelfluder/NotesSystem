using SharpContainerProg.Register;
using Unity.Injection;

namespace SharpContainerProg.AAPublic
{
    public abstract class RegistrationBase
    {
        public static IContainer container = SetContainerStatic();
        private bool registrationStarted;

        private static IContainer SetContainerStatic()
        {
            if (container != null)
            {
                return container;
            }

            return new Container();
        }

        private void SetContainer()
        {
            if (container == null)
            {
                container = new Container();
            }
        }

        public RegistrationBase()
        {
            SetContainer();
        }

        public IContainer Start()
        {
            if (!registrationStarted)
            {
                registrationStarted = true;
                Registrations();
                registrationStarted = false;
            }

            return container;
        }

        public IContainer Start(ref bool isRegistered)
        {
            if (!registrationStarted
                && !isRegistered)
            {
                registrationStarted = true;
                Registrations();
                isRegistered = true;
                registrationStarted = false;
            }

            return container;
        }

        public abstract void Registrations();

        public void RegisterByFunc<RegT>(Func<RegT> func, int type = 0)
        {
            var factory = new InjectionFactory(c =>
            {
                return func.Invoke();
            });
            if (type == 0)
            {
                container.RegisterSingleton<RegT>(factory);
            }
            if (type == 1)
            {
                container.RegisterType<RegT>(factory);
            }
        }

        public void RegisterByFunc<RegT, ParT1>(
            Func<ParT1, RegT> func, ParT1 t1)
        {
            container.RegisterSingleton<RegT>(new InjectionFactory(c =>
            {
                return func.Invoke(t1);
            }));
        }

        public void RegisterByFunc<RegT, ParT1>(
            Func<ParT1, RegT> rfunc,
            Func<ParT1> arg1func)
        {
            container.RegisterSingleton<RegT>(new InjectionFactory(c =>
            {
                return rfunc.Invoke(arg1func.Invoke());
            }));
        }

        public void RegisterByFunc<RegT, ParT1, ParT2>(
            Func<ParT1, ParT2, RegT> rfunc,
            ParT1 p1, ParT2 p2)
        {
            container.RegisterType<RegT>(new InjectionFactory(c =>
            {
                return rfunc.Invoke(p1, p2);
            }));
        }
    }
}
