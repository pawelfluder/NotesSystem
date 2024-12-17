using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;

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

    public (string Repo, string Loca) GetFirstRepo()
    {
        string firstRepo = Path.GetFileName(reposPathsList.First());
        return (firstRepo, "");
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

    public void GetGroupsFromSearchPaths(List<string> searchPaths)
    {
        reposPathsList = new List<string>();
        Dictionary<string, List<string>> dict = GetGuidGroupsForSearchFolders(searchPaths);
        AddRepoFolders(dict);
        reposPathsList = dict.SelectMany(x => x.Value).ToList();
    }

    private Dictionary<string, List<string>> GetGuidGroupsForSearchFolders(
        List<string> searchFolders)
    {
        Dictionary<string, List<string>> dict = new();
        foreach (var searchFolder in searchFolders)
        {
            List<string> possibleGuidFolders = Directory.GetDirectories(searchFolder)
                .Select(x => CorrectPath(x))
                .ToList();
            foreach (var possibleGuidFolder in possibleGuidFolders)
            {
                if (IsUniRepoGroupFolder(possibleGuidFolder))
                {
                    if (!dict.ContainsKey(possibleGuidFolder))
                    {
                        dict.Add(possibleGuidFolder, new List<string>());
                    }
                }
            }
        }

        return dict;
    }

    private void AddRepoFolders(
        Dictionary<string, List<string>> dict)
    {
        foreach (var keyValue in dict)
        {
            string guidFolder = keyValue.Key;
            List<string> repoFolders = Directory.GetDirectories(guidFolder)
                .Select(x => CorrectPath(x))
                .ToList();
            dict[guidFolder].AddRange(repoFolders);
        }
    }

    private bool IsUniRepoGroupFolder(string folder)
    {
        string name = Path.GetFileName(folder);
        bool isGuid = Guid.TryParse(name, out Guid guid);
        return isGuid;
    }

    public string CorrectPath(string path)
    {
        return path.Replace("\\", "/");
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
