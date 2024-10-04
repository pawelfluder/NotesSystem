using System.Reflection;

namespace SharpOperationsProg.AAPublic;

public class StrategyBase<T>
{
    protected List<Type> _possibleStrategies;
    protected T _strategy;
    
    private AssemblyName GetAssemblyName(object obj)
    {
        var assembly = Assembly.GetAssembly(obj.GetType());
        var assebmlyName = assembly.GetName();
        return assebmlyName;
    }
    
    public StrategyBase(object parent)
    {
        Type interfaceType = typeof(T);
        AssemblyName assemblyName = GetAssemblyName(parent);
        Assembly assembly = Assembly.Load(assemblyName);
        _possibleStrategies = assembly.GetTypes()
            .Where(mytype => mytype.GetInterfaces().Contains(interfaceType))
            .ToList();
        
        // wrong
        // var types = assembly.GetTypes()
        //     .Where(p => p.IsAssignableFrom(interfaceType))
        //     .ToList();
    }
    
    public T GetNewStrategy(string name)
    {
        if (_possibleStrategies.Count < 1)
        {
            throw new Exception();
        }

        var match = _possibleStrategies.Where(x => x.Name == name);
        if (match.Count() != 1)
        {
            throw new Exception();
        }
        
        Type type = _possibleStrategies.Single(x => x.Name == name);
        var obj = Activator.CreateInstance(type);
        var newStrategy = (T)obj;
        return newStrategy;
    }

    public bool SetNewStrategy(string name)
    {
        var newStrategy = GetNewStrategy(name);
        if (default(T).Equals(newStrategy))
        {
            return false;
        }
        
        _strategy = newStrategy;
        return true;
    }
}