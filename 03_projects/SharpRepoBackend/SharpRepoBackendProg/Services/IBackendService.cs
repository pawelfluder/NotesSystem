namespace SharpRepoBackendProg.Services;

public interface IBackendService
{
    // string CommandApi(string cmdName, string repo = "", string loca = "");
    string InvokeStringArgsApi(params string[] args);
    // string RepoApi(string repo, string inputLoca);
    // string RepoApi(string methodName, params string[] args);
}