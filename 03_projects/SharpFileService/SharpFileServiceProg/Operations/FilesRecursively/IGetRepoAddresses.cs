namespace SharpRepoServiceProg.FileOperations
{
    public interface IRepoAddressesObtainer
    {
        List<string> Visit(string path);
    }
}