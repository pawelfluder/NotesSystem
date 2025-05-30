﻿using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpConfigProg.AAPublic;

public partial interface IConfigService
{
    public interface ILocalProgramDataPreparer : IPreparer { }
}

public class GuidFolderPreparer : IPreparer
{
    private readonly IOperationsService operationsService;
    private IConfigService configService;

    public GuidFolderPreparer(IOperationsService operationsService)
    {
        this.operationsService = operationsService;
    }

    public Dictionary<string, object> Prepare()
    {
        var paths = new Dictionary<string, object>();

        var repoRootPaths = GetRepoSearchPaths2();
        paths = new Dictionary<string, object>()
        {
            { "repoRootPaths", repoRootPaths},
        };
        paths.ToList().ForEach(x => Console.WriteLine(x.Key + ": " + x.Value));

        return paths;
    }

    public List<object> GetRepoSearchPaths2()
    {
        var programDataFolderPath = operationsService.Path.GetProjectFolderPath("18296f12-0706-43e1-9bd4-1b40154ec22e");
        var searchPaths = new List<object> { programDataFolderPath };
        return searchPaths;
    }

    public void SetConfigService(IConfigService configService)
    {
        this.configService = configService;
    }
}