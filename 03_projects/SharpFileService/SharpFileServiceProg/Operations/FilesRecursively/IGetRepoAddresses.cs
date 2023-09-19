namespace SharpRepoServiceProg.FileOperations
{
    public interface IRepoAddressesObtainer
    {
        List<(string, string)> Visit(string path);
    }
}