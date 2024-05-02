using System;

namespace SharpFileServiceProg.Operations.RepoAddress
{
    public interface IRepoAddressOperations
    {
        public string JoinLoca(string loca01, string loca02);
        public (string, string) AdrTupleJoinLoca((string Repo, string Loca) adrTuple, string loca);
        (string, string) CreateAddressFromString(string addressString);
        Uri CreateUriFromAddress((string Repo, string Loca) address, int index);
        string CreateUrlFromAddress((string Repo, string Loca) address);
        string MoveOneLocaBack(string adrString);
        int GetLastLocaIndex(string addressString);
    }
}