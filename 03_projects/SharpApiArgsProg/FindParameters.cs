using System.Reflection;

namespace SharpApiArgsProg;

public class FindParameters
{
    public object Try(
        string[] args,
        MethodInfo info)
    {
        var parameters = args.Skip(3).ToArray();
        var gg = GetParameters(parameters, info);
        return gg;
    }

    private List<object> GetParameters(
        string[] parameters,
        MethodInfo info)
    {
        var resultParList = new List<object>();
        var infoParList = info.GetParameters();
        for (int i = 0; i < infoParList.Length; i++)
        {
            var strParameter = parameters[i];
            var infoParameter = infoParList[i];
            
            var type = infoParameter.ParameterType;
            var par = ConvertParamFromString(strParameter, infoParameter.ParameterType);
            resultParList.Add(par);
        }

        return resultParList;
    }

    private object ConvertParamFromString(
        string value,
        Type targetType)
    {
        if (targetType == typeof(string))
            return value;

        if (targetType == typeof(int))
            return int.Parse(value);

        if (targetType == typeof(long))
            return long.Parse(value);

        if (targetType == typeof(ulong))
            return ulong.Parse(value);

        if (targetType == typeof(bool))
            return bool.Parse(value);

        if (targetType == typeof(Guid))
            return Guid.Parse(value);

        if (targetType == typeof(DateTime))
            return DateTime.Parse(value);

        if (targetType.IsEnum)
            return Enum.Parse(targetType, value, ignoreCase: true);

        // Nullable<T>
        if (Nullable.GetUnderlyingType(targetType) is Type underlyingType)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return ConvertParamFromString(value, underlyingType);
        }

        // Fallback - spróbuj Convert.ChangeType
        return Convert.ChangeType(value, targetType);
    }
}