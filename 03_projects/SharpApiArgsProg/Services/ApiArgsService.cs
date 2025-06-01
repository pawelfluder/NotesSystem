using SharpApiArgsProg.Registrations;
using SharpArgsManagerProj.AAPublic;
using SharpRepoServiceProg.AAPublic;

namespace SharpApiArgsProg.Services;

public class ApiArgsService : IArgsManagerService
{
    private Dictionary<string, object> _storeOfServices;
    private readonly FindWorker _findWorker;
    private readonly FindMethod _findMethod;
    private readonly FindParameters _findParameters;

    public ApiArgsService()
    {
        _findWorker = new FindWorker();
        _findMethod = new FindMethod();
        _findParameters = new FindParameters();
        List<object> servicesList = new()
        {
            MyBorder.OutContainer.Resolve<IRepoService>(),
        };
        
        _storeOfServices = servicesList
            .ToDictionary(x => GetInterface(x.GetType()), x => x);
        
    }

    private string GetInterface(
        Type type)
    {
        var interfaces = type.GetInterfaces();
        if (interfaces.Length == 0)
            return "";
        
        return interfaces[0].Name;
    }

    public string Resolve(string[] args)
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
        var service = _storeOfServices[args[0]];

        if (service == null)
        {
            return "";
        }
        
        var worker = _findWorker.Try(args, service);
        
        if (worker == null)
        {
            return "";
        }
        
        var method = _findMethod.Try(args, worker);
        
        if (method == null)
        {
            return "";
        }
        
        var parameters = _findParameters.Try(args, method);
        
        if (parameters == null)
        {
            return "";
        }

        //method.Invoke(worker, parameters);
        return "";
    }

    private void PrintAvailableMethods()
    {
        throw new NotImplementedException();
    }
}
