namespace SharpOperationsProg.Operations.RepoAddress
{
    internal class RepoAddressOperations : IRepoAddressOperations
    {
        private readonly IIndexWrk indexOperations;

        public RepoAddressOperations(IIndexWrk indexOperations)
        {
            this.indexOperations = indexOperations;
        }

        public (string, string) AdrTupleJoinLoca(
            (string Repo, string Loca) adrTuple, string loca)
        {
            if (loca == string.Empty)
            {
                return adrTuple;
            }

            var newLoca = JoinLoca(adrTuple.Loca, loca);
            var newAdrTuple = (adrTuple.Repo, newLoca);
            return newAdrTuple;
        }

        public string JoinLoca(string loca01, string loca02)
        {
            if (loca01 == string.Empty)
            {
                return loca02;
            }

            var newLoca = loca01 + "/" + loca02;
            return newLoca;
        }

        public Uri CreateUriFromAddress((string Repo, string Loca) address, int index)
        {
            var indexString = indexOperations.IndexToString(index);
            if (address.Loca != string.Empty)
            {
                address = (address.Repo, address.Loca + "/" + indexString);
            }

            if (address.Loca == string.Empty)
            {
                address = (address.Repo, indexString);
            }

            var url = CreateUrlFromAddress(address);
            var url2 = "https://" + url;
            var uri = new Uri(url2);

            if (url.Contains("//"))
            {
                throw new Exception();
            }

            return uri;
        }

        public string CreateUrlFromAddress((string Repo, string Loca) address)
        {
            if (address.Loca == string.Empty)
            {
                return address.Repo;
            }

            var url = address.Repo + "/" + address.Loca;
            return url;
        }

        public string MoveOneLocaBack(string adrString)
        {
            var slashCount = adrString.Count(x => x == '/');
            if (slashCount == 0)
            {
                return adrString;
            }
            
            var splited = adrString.Split("/").ToList();
            var lastItemIndex = splited.Count - 1;
            splited.RemoveAt(lastItemIndex);
            var newAddress = String.Join('/', splited);

            return newAddress;
        }

        public int GetLastLocaIndex(string addressString)
        {
            var slashCount = addressString.Count(x => x == '/');
            if (slashCount < 1)
            {
                throw new Exception();
            }

            var splited = addressString.Split("/").ToList();
            var lastIndexString = splited.Last();
            var lastIndex = indexOperations.StringToIndex(lastIndexString);
            return lastIndex;
        }
        public (string, string) CreateAddressFromString(string addressString)
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
}