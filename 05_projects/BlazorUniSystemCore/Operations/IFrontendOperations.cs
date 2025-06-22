using BlazorUniSystemCore.Operations.Json;
using BlazorUniSystemCore.Operations.NoSqlAddress;
using BlazorUniSystemCore.Operations.TwoDigitsString;

namespace BlazorUniSystemCore.Operations;

public interface IFrontendOperations
{
    public static ITwoDigitsStringOperations TwoDigitsStr { get; }
        = new TwoDigitsStringOperations();

    public static INoSqlAddressOperations NoSqlAddress { get; }
        = new NoSqlAddressOperations();
    
    public static IJsonOperations Json { get; }
        = new JsonOperations();
}
