using static SharpFileServiceProg.Service.IFileService;

namespace SharpFileServiceProg.Operations.RepoAddress
{
    internal class RepoAddressOperations : IRepoAddressOperations
    {
        private readonly IIndexWrk indexOperations;

        public RepoAddressOperations(IIndexWrk indexOperations)
        {
            this.indexOperations = indexOperations;
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
