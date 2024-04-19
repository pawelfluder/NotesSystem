using System.Reflection;

namespace SharpFileServiceProg.AAPublic
{
    public interface IReflectionOperations
    {
        IEnumerable<(string, string)> GetPropTuples(object obj);
        List<string> GetPropNames<T>(params string[] propArray);
        bool HasProp<T>(params string[] propArray);
        List<PropertyInfo> GetPropList<T>(params string[] propArray);
    }
}