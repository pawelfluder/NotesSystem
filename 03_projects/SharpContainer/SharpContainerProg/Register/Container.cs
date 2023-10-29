using SharpContainerProg.AAPublic;
using System.Reflection;
using Unity;
using Unity.Injection;

namespace SharpContainerProg.Register
{
    internal class Container : IContainer
    {
        private UnityContainer unity = new UnityContainer();
        private static bool nLogLoaded = LoadNLogConfig();

        private static bool LoadNLogConfig()
        {
            try
            {
                string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(assemblyFolder + "\\NLog.config");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsRegistered<T>()
        {
            var result = unity.IsRegistered(typeof(T));
            return result;
        }

        public T Resolve<T>()
        {
            var result = unity.Resolve<T>();
            return result;

        }

        public object Resolve(Type type)
        {
            var result = unity.Resolve(type);
            return result;
        }

        public IContainer RegisterSingleton<T>(params object[] injectionMember)
        {
            var tmp = injectionMember.Select(x => (InjectionMember)x).ToArray();
            var result = unity.RegisterSingleton<T>(tmp);
            return this;
        }

        public IContainer RegisterType<T>(params object[] injectionMember)
        {
            var tmp = injectionMember.Select(x => (InjectionMember)x).ToArray();
            var result = unity.RegisterType<T>(tmp);
            return this;
        }
    }
}
