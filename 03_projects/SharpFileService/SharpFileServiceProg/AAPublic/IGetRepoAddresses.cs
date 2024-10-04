namespace SharpFileServiceProg.AAPublic
{
    public interface IRepoAddressesObtainer
    {
        List<string> Visit(string path);
    }
}