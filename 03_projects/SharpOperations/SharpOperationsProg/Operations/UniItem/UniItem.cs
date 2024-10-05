using SharpOperationsProg.Operations.UniAddress;

namespace SharpOperationsProg.Operations.UniItem;

internal class UniItem
{
    private string _address;

    private Dictionary<string, object> _settings;

    internal string Name { get; set; }

    internal string Type { get; set; }

    internal string Id { get; set; }

    internal string Address
    {
        get => _address;
        set
        {
            _address = value;
            var adrTuple = IUniAddressOperations.CreateAddressFromString(_address);
            AdrTuple = adrTuple;
        }
    }

    internal (string Repo, string Loca) AdrTuple { get; private set; }

    public object Body { get; set; }

    public Dictionary<string, object> Settings
    {
        get => _settings;
        set
        {
            _settings = value;
            SetIndentificators(_settings);
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