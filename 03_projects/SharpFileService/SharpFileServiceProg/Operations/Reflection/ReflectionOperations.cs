using SharpFileServiceProg.AAPublic;
using System.Reflection;

namespace SharpFileServiceProg.Operations.Reflection
{
    public class ReflectionOperations : IReflectionOperations
    {
        public IEnumerable<(string, string)> GetPropTuples(object obj)
        {
            var properties = obj.GetType().GetProperties();
            var tuples = properties.Select(x => (x.Name, x.GetValue(obj).ToString()));
            return tuples;
        }

        public List<string> GetPropNames<T>(params string[] propArray)
        {
            var type = typeof(T);
            var propNames = type.GetProperties().Select(x => x.Name).ToList();
            return propNames;
        }

        public List<PropertyInfo> GetPropList<T>(params string[] propArray)
        {
            var type = typeof(T);
            var propList = type.GetProperties().ToList();
            return propList;
        }

        public bool HasProp<T>(params string[] propArray)
        {
            var type = typeof(T);
            var propNames = type.GetProperties().Select(x => x.Name).ToList();
            foreach (var prop in propArray)
            {
                if (!propNames.Any(x => x == prop))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
