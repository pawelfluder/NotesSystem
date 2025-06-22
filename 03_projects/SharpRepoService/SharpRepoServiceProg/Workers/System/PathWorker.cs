using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SharpRepoServiceProg.Helpers;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registrations;

namespace SharpRepoServiceProg.Workers.System;

internal class PathWorker
{
    public List<string> reposPathsList;
    private string slash = "/";

    private string contentFileName;
    private string configFileName;
    private readonly CustomOperationsService _operations;

    public PathWorker()
    {
        SetNames();
        _operations = MyBorder.MyContainer.Resolve<CustomOperationsService>();
    }

    public List<string> GetAllReposPaths() => reposPathsList;

    private void SetNames()
    {
        configFileName = "nazwa.txt";
        contentFileName = "lista.txt";
    }

    public string[] GetFirstRepo()
    {
        string firstRepo = Path.GetFileName(reposPathsList.First());
        return [firstRepo, ""];
    }

    public string GetItemPath(
        (string Repo, string Loca) adrTuple)
    {
        var elemPath = GetRepoPath(adrTuple.Repo);
        if (adrTuple.Loca != string.Empty)
        {
            elemPath += slash + adrTuple.Loca;
        }

        return elemPath;
    }

    public string GetRepoPath(string repo)
    {
        List<string> foundList = reposPathsList
            .Where(x => Path.GetFileName(x) == repo).ToList();

        if (foundList != null &&
            foundList.Count() == 1)
        {
            string result = foundList.First();
            return result;
        }

        string result2 = HandleError();
        return result2;
    }

    public string GetConfigPath(
        (string Repo, string Location) address)
    {
        string tmp = GetItemPath(address);
        string path = tmp + slash + configFileName;
        return path;
    }

    public string GetBodyPath((string Name, string Location) address)
    {
        string tmp = GetItemPath(address);
        string path = tmp + slash + contentFileName;
        return path;
    }

    public void GetGroupsFromSearchPaths(
        List<string> searchPaths)
    {
        reposPathsList = new List<string>();
        GuidGroupsHelper helper = new();
        List<string> specialFolders = helper.GetSpecialWithGuidFolders(searchPaths);
        searchPaths.AddRange(specialFolders);
        Dictionary<string, List<string>> dict = helper.
            GetGuidGroupsForSearchFolders(searchPaths);
        helper.AddRepoFolders(dict);
        reposPathsList = dict.SelectMany(x => x.Value).ToList();
        reposPathsList.Sort(new RepoPathComparer());
    }

    private string HandleError()
    {
        throw new InvalidOperationException();
    }

    internal int GetRepoCount()
    {
        return reposPathsList.Count();
    }
}
