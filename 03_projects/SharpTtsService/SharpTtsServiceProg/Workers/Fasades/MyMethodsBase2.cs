namespace SharpTtsServiceProg.Workers.Fasades;

public class MyMethodsBase2
{
    private List<string> methodNames;
    protected object? _obj;
    protected Type _objType;

    public MyMethodsBase2(object obj = null)
    {
        _obj = obj;
        _obj ??= this;
        _objType = obj.GetType();
        methodNames = SetMethodNames();
    }

    public virtual async Task RunMethodAsync(
        string methodName, params object[] args)
    {
    }

    public List<string> GetMethodNames()
    {
        return methodNames;
    }

    private List<string> SetMethodNames()
    {
        var result = _objType.GetMethods()
            .Select(m => m.Name).ToList();

        result.Remove("GetType");
        result.Remove("GetHashCode");
        result.Remove("ToString");
        result.Remove("Equals");
        result.Remove("RunMethodAsync");
        result.Remove("GetMethodNames");

        return result;
    }
}