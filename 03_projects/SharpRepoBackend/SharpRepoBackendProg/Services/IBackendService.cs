namespace SharpRepoBackendProg.Services;

public partial interface IBackendService
{
    // string CommandApi(string cmdName, string repo = "", string loca = "");
    string CommandApi(string cmdName, params string[] args);
    string RepoApi(string repo, string loca);
    string RepoApi(string methodName, params string[] args);
}