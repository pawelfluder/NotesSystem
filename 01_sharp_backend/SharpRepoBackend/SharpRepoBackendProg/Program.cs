using SharpRepoBackendProg;

class Program
{
    static void Main(string[] args)
    {
        var backendService = new BackendService();
        backendService.Run(args);
    }
}