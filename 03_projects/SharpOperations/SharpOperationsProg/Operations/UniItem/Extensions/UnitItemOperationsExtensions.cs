using Newtonsoft.Json;
using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.Operations.UniAddress;
using SharpRepoServiceProg.AAPublic;

namespace SharpOperationsProg.Operations.UniItem.Extensions;

public static class UnitItemExtensions
{
    public static (string Repo, string Loca) GetAdrTupleByNamesList(
        this IRepoService repoService,
        (string Repo, string Loca) mainAdrTuple,
        List<string> names)
    {
        var adrTuple = repoService.Methods.GetAdrTupleByNameList(mainAdrTuple, names);
        return adrTuple;
    }
    
    public static (string Repo, string Loca) GetAdrTuple<T>(
        this IRepoService repoService,
        (string Repo, string Loca) mainAdrTuple)
    {
        string name = typeof(T).Name;
        string? json = repoService.Item
            .PostItem(mainAdrTuple, "Folder", name);
        UniItem? uniItem = JsonConvert.DeserializeObject<UniItem>(json);
        if (uniItem == null) { return default; }
        (string Repo, string Loca) adrTuple = uniItem.AdrTuple;
        return adrTuple;
    }
    public static (string, string) CreateItem(
        this IRepoService repoService,
        (string, string) parentAdrTuple,
        string type,
        string name)
    {
        string? json = repoService.Item.PostItem(parentAdrTuple, type, name);
        UniItem? uniItem = JsonConvert.DeserializeObject<UniItem>(json);
        return uniItem.AdrTuple;
    }
    
    public static (string, string) CreateItem(
        this IRepoService repoService,
        (string, string) parentAdrTuple,
        string type,
        string name,
        string body)
    {
        string? json = repoService.Item.PutItem(parentAdrTuple, type, name, body);
        UniItem? uniItem = JsonConvert.DeserializeObject<UniItem>(json);
        return uniItem.AdrTuple;
    }

    public static (string, string) CreateFolder(
        this IRepoService repoService,
        (string, string) parentAdrTuple,
        string name)
    {
        var adrTuple = CreateItem(repoService, parentAdrTuple, "Folder", name);
        return adrTuple;
    }
    
    public static Dictionary<string, object> GetSettings(
        this IRepoService repoService,
        (string, string) adrTuple)
    {
        var json = repoService.Item.GetItem(adrTuple);
        var item = JsonConvert.DeserializeObject<UniItem>(json);
        var setting = item.Settings;
        return setting;
    }
    
    public static string PutObjListAsText<T>(
        this IRepoService repoService,
        (string Repo, string Loca) adrTuple,
        IEnumerable<T> objList)
    {
        var name = typeof(T).Name;
        var body = IYamlDefaultOperations.Serialize(objList);
        var outAdrTuple = repoService.Item.PutItem(adrTuple, "Text", name, body);
        return body;
    }
    
    public static IEnumerable<T> GetObjListFromText<T>(
        this IRepoService repoService,
        (string, string) typeAdrTuple)
    {
        var objectsList = repoService.GetItemList<T>(typeAdrTuple);
        return objectsList;
    }
    
    public static string GetBody(
        this IRepoService repoService,
        (string, string) adrTuple)
    {
        var json = repoService.Item.GetItem(adrTuple);
        var item = JsonConvert.DeserializeObject<UniItem>(json);
        return item.Body.ToString();
    }
    
    public static (string, Dictionary<string, object>) GetBodyQSettings(
        this IRepoService repoService,
        (string, string) adrTuple)
    {
        var json = repoService.Item.GetItem(adrTuple);
        var item = JsonConvert.DeserializeObject<UniItem>(json);
        var bodyQsettings = (item.Body.ToString(), item.Settings);
        return bodyQsettings;
    }
    
    public static (string, string) OverrideTextItem(
        this IRepoService repoService,
        (string, string) adrTuple,
        string name,
        List<string> textLines)
    {
        // todo
        // repoService.Item.DeleteItem(adrTuple);
        var adrTuple02 = repoService.Methods
            .PutText(adrTuple, name, string.Join('\n', textLines));
        return adrTuple02;
    }
    
    public static (string, string) CreateText(
        this IRepoService repoService,
        (string, string) parentAdrTuple,
        string name)
    {
        var adrTuple = CreateItem(repoService, parentAdrTuple, "Text", name);
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
                .PostItem(parentAdrTuple, "Text", name);
        }

        var objectsList = repoService.GetItemList<T>(typeAdrTuple);
        return objectsList;
    }
    
    public static IEnumerable<T> GetUniItemList<T>(
        this IRepoService repoService,
        (string Repo, string Loca) adrTuple)
    {
        string? json = repoService.Item.GetItemList(adrTuple);
        List<T> itemList = IYamlDefaultOperations.Deserialize<List<T>>(json);
        return itemList;
    }
    
    public static IEnumerable<T> GetItemList<T>(
        this IRepoService repoService,
        (string Repo, string Loca) adrTuple)
    {
        string? jsonString = repoService.Methods.GetItem(adrTuple);
        UniItem? item = JsonConvert.DeserializeObject<UniItem>(jsonString);
        string? body = item.Body.ToString();
        List<T> itemList = IYamlDefaultOperations.Deserialize<List<T>>(body);
        return itemList;
    }
}