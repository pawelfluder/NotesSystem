using System;

namespace SharpFileServiceProg.Operations.RepoAddress
{
    public interface IRepoAddressOperations
    {
        (string, string) CreateAddressFromString(string addressString);
        Uri CreateUriFromAddress((string Repo, string Loca) address, int index);
        string CreateUrlFromAddress((string Repo, string Loca) address);
        string MoveOneLocaBack(string adrString);
    }
}