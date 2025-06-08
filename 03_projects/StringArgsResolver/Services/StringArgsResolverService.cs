using System.Reflection;
using SharpApiArgsProg.AAPublic;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.Operations.Reflection;

namespace SharpApiArgsProg.Services;

public class StringArgsResolverService : IStringArgsResolverService
{
    private Dictionary<string, object> _storeOfServices;
    private readonly FindWorker _findWorker;
    private readonly FindMethod _findMethod;
    private readonly FindParameters _findParameters;
    private readonly IReflectionOpV2 _reflection;

    public StringArgsResolverService(
        List<object> servicesList)
    {
        _findWorker = new FindWorker();
        _findMethod = new FindMethod();
        _findParameters = new FindParameters();
        _reflection = IOperationsService.ReflectionV2;
        
        _storeOfServices = servicesList
            .ToDictionary(x => _reflection.GetInterface(x.GetType()), x => x);
    }

    public string Invoke(string[] args)
    {
        if (args.Length == 0)
        {
            return "";
        }

        if (args.Length == 1)
        {
            PrintAvailableMethods();
        }

        var result = TryRunMethod(args);
        return result;
    }

    private string TryRunMethod(string[] args)
    {
        object? service = _storeOfServices[args[0]];
        if (service == null) return "";
        
        object? worker = _findWorker.Try(args, service);
        if (worker == null) return "";

        MethodInfo? method = _findMethod.Try(args, worker);
        if (method == null) return "";
        
        object[]? parameters = _findParameters.Try(args, method);
        if (parameters == null) return "";
        
        string result = TryInvoke(worker, method, parameters);
        return result;
    }

    private void PrintAvailableMethods()
    {
    }
    
    private string TryInvoke(
        object worker,
        MethodInfo method,
        object[] parameters)
    {
        try
        {
            object? result = method.Invoke(worker, parameters);
            string? json = result.ToString();
            return json;
        }
        catch (Exception e)
        {
            return "";
        }
    }
}
