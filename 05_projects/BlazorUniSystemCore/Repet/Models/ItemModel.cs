﻿using SharpOperationsProg.Operations.UniAddress;
using SharpRepoServiceProg.AAPublic.Names;

namespace BlazorUniSystemCore.Repet.Models;

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
        Name = dict[ConfigKeys.Name].ToString();
        Id = dict[ConfigKeys.Id].ToString();
        Type = dict[ConfigKeys.Type].ToString();
        Address = dict[ConfigKeys.Address].ToString();
    }
}
