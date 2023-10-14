using SharpContainerProg.Public;
using System.Reflection;
using Unity;
using Unity.Injection;

namespace SharpContainerProg.Repetition
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
            var result = UnityContainerExtensions.IsRegistered(unity, typeof(T));
            return result;
        }

        public T Resolve<T>()
        {
            var result = UnityContainerExtensions.Resolve<T>(unity);
            return result;
            
        }

        public object Resolve(Type type)
        {
            var result = UnityContainerExtensions.Resolve(unity, type);
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
