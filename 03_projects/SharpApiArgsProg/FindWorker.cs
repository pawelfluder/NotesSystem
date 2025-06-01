using System.Reflection;
using SharpOperationsProg.AAPublic;

namespace SharpApiArgsProg;

internal class FindWorker
{
    public object Try(
        string[] args,
        object service)
    {
        string workerName = args[1];
        object worker = GetProperty(service, workerName);
        return worker;
    }

    private object GetProperty(
        object service,
        string propName)
    {
        var infoList = service.GetType().GetProperties();
        PropertyInfo foundInfo = null;
        foreach (var info in infoList)
        {
            var interName = IOperationsService.ReflectionV2.GetInterface(info.PropertyType);
            if (info.PropertyType.Name == propName)
            {
                foundInfo = info;
                break;
            }
        }
        
        //var gg = service.GetType().GetProperty()
        
        var prop = foundInfo.GetValue(service);
        return prop ?? new Object();
    }
}
