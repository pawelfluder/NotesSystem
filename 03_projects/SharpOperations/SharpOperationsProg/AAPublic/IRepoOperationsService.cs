using SharpOperationsProg.Operations.UniAddress;
using SharpOperationsProg.Operations.UniItem;

namespace SharpOperationsProg.AAPublic;

public interface IRepoOperationsService
{
    public UnitItemOperations Item { get; }
    
    public IUniAddressOperations Address { get; }
}
