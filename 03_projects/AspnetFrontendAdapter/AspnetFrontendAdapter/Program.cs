using SharpRepoBackendProg;

class Program
{
    static void Main(string[] args)
    {
        var frontendAdapter = new FrontendAdapter();
        frontendAdapter.Run(args);
    }
}