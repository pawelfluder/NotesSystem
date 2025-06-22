using System.Collections.Generic;

namespace WpfNotesSystemProg3.Models;

public class RepoItem
{
    public string Name { get; set; }

    public string Type { get; set; }

    public string Id { get; set; }

    public object Body { get; set; }

    public string Address { get; set; }

    private Dictionary<string, object> settings;

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
        Name = dict["name"].ToString();
        Type = dict["type"].ToString();
        Id = dict["id"].ToString();
        // Address = dict["address"].ToString();
    }
}