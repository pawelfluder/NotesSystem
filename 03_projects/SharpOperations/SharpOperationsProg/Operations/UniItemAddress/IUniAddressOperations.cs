namespace SharpOperationsProg.Operations.UniAddress;

public interface IUniAddressOperations
{
    public string JoinLoca(string loca01, string loca02);
    public (string, string) AdrTupleJoinLoca((string Repo, string Loca) adrTuple, string loca);
    //(string, string) CreateAddressFromString(string addressString);
    Uri CreateUriFromAddress((string Repo, string Loca) address, int index);
    string CreateUrlFromAddress((string Repo, string Loca) address);
    string MoveOneLocaBack(string adrString);
    int GetLastLocaIndex(string addressString);
    static (string, string) CreateAddressFromString(string addressString)
    {
        addressString = addressString.Trim('/').Replace("https://", "");
        var index = addressString.IndexOf('/');
        if (!addressString.Contains('/'))
        {
            return (addressString, "");
        }

        var repo = addressString.Substring(0, index);
        var loca = addressString.Substring(index + 1, addressString.Length - index - 1);

        if (loca.StartsWith('/'))
        {
            throw new Exception();
        }

        return (repo, loca);
    }
}