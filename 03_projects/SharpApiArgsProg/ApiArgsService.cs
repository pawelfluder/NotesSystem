using SharpApiArgsProg.Registrations;
using SharpArgsManagerProj.AAPublic;
using SharpRepoServiceProg.AAPublic;

namespace SharpApiArgsProg;

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
            .ToDictionary(x => x.ToString(), x => x);
    }

    public void Resolve(string[] args)
    {
        if (args.Length == 0)
        {
            return;
        }

        if (args.Length == 1)
        {
            PrintAvailableMethods();
        }

        TryRunMethod(args);
    }

    private void TryRunMethod(string[] args)
    {
        List<string> services = _storeOfServices.Keys.ToList();
        string? service = services.SingleOrDefault(x => x == args[0]);

        if (service == null)
        {
            return;
        }
        
        var worker = _findWorker.Try(args);
        
        if (worker == null)
        {
            return;
        }
        
        var method = _findMethod.Try(args);
        
        if (worker == null)
        {
            return;
        }
        
        var parameters = _findParameters.Try(args);
        
        if (parameters == null)
        {
            return;
        }

        //method.Invoke(worker, parameters);
    }

    private void PrintAvailableMethods()
    {
        throw new NotImplementedException();
    }
}
