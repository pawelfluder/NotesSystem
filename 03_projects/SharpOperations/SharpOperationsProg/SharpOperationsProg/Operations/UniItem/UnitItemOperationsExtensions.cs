using Newtonsoft.Json;
using SharpRepoServiceProg.AAPublic;

namespace SharpOperationsProg.Operations.UniItem;

public static class UnitItemOperationExtensions
{
    public static (string Repo, string Loca) GetAdrTuple<T>(
        this IRepoService repoService,
        (string Repo, string Loca) mainAdrTuple)
    {
        string name = typeof(T).Name;
        string? json = repoService.Item
            .CreateItem(mainAdrTuple, "Folder", name);
        UniItem? uniItem = JsonConvert.DeserializeObject<UniItem>(json);
        if (uniItem == null) { return default; }
        (string Repo, string Loca) adrTuple = uniItem.AdrTuple;
        return adrTuple;
    }
    
    public static IEnumerable<T> GetItemList<T>(
        this IRepoService repoService,
        string parentAddress,
        string name)
    {
        var parentAdrTuple = IOperationService.RepoAddress
            .CreateAddressFromString(parentAddress);
        var typeAdrTuple = repoService.Methods
            .GetAdrTupleByName(parentAdrTuple, name);

        if (typeAdrTuple == default)
        {
            repoService.Item
                .CreateItem(parentAdrTuple, "Text", name);
        }

        var objectsList = repoService.GetItemList<T>(typeAdrTuple);
        return objectsList;
    }
}