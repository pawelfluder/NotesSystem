using System;
using System.Collections.Generic;
using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.AAPublic;
using SharpRepoServiceProg.Infos;
using SharpRepoServiceProg.Workers.Public;

namespace SharpRepoServiceProg.Service;

internal class RepoService : IRepoService
{
    private readonly ServerInfo serverInfo;
    private readonly IFileService fileService;
    private readonly LocalInfo localInfo;
    private bool repoWorkerInit;
    private bool itemWorkerInit;

    private MethodWorker methods;
    private JsonWorker item;

    private bool isRepoInit;

    internal RepoService(
        IFileService fileService)
    {
        localInfo = new LocalInfo(null);
        serverInfo = null;
        this.fileService = fileService;
    }

    public MethodWorker Methods
    {
        get
        {
            if (!isRepoInit)
            {
                methods = new MethodWorker(fileService, serverInfo, localInfo);
                isRepoInit = true;
            }

            return methods;
        }
    }

    public JsonWorker Item
    {
        get
        {
            if (!itemWorkerInit)
            {
                item = new JsonWorker();
                itemWorkerInit = true;
            }

            return item;
        }
    }

    public void InitGroupsFromSearchPaths(List<string> searchPaths)
    {
        Methods.InitGroupsFromSearchPaths(searchPaths);

        if (!(methods.GetReposCount() > 0))
        {
            throw new Exception();
        }
    }
    
    public (string Repo, string Loca) GetFirstRepo()
    {
        (string Repos, string Loca) adrTuple = Methods.GetFirstRepo();
        return adrTuple;
    }
}