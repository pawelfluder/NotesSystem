using SharpOperationsProg.Operations.UniAddress;
using SharpRepoServiceProg.Names;
namespace BlazorNotesSystem.Repet.Models;

public class ItemModel
{
    private string address;

    private Dictionary<string, object> settings;

    internal string Name { get; set; }

    internal string Type { get; set; }

    internal string Id { get; set; }

    internal string Address
    {
        get => address;
        set
        {
            address = value;
            (string, string) adrTuple = IUniAddressOperations.CreateAddressFromString(address);
            AdrTuple = adrTuple;
        }
    }

    public (string Repo, string Loca) AdrTuple { get; set; }

    public object Body { get; set; }

    public Dictionary<string, object> Settings
    {
        get => settings;
        set
        {
            settings = value;
            SetIndentificators(settings);
        }
    }

    private void SetIndentificators(
        Dictionary<string, object> dict)
    {
        Name = dict[FieldsForUniItem.Name].ToString();
        Id = dict[FieldsForUniItem.Id].ToString();
        Type = dict[FieldsForUniItem.Type].ToString();
        Address = dict[FieldsForUniItem.Address].ToString();
    }
}
