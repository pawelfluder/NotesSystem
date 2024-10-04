using Newtonsoft.Json;
using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.Operations.UniAddress;
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
        var parentAdrTuple = IUniAddressOperations
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
    
    public static IEnumerable<T> GetItemList<T>(
        this IRepoService repoService,
        (string Repo, string Loca) adrTuple)
    {
        var jsonString = repoService.Methods.GetItem(adrTuple);
        var item = JsonConvert.DeserializeObject<UniItem>(jsonString);
        var body = item.Body.ToString();
        var itemList = IYamlDefaultOperations.Deserialize<List<T>>(body);
        return itemList;
    }
}