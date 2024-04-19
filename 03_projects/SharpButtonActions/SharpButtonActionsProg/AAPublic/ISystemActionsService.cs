namespace SharpButtonActionsProg.AAPublic
{
    public interface ISystemActionsService
    {
        void OpenFile(string path);
        void OpenFolder(string path);
        void OpenTerminal(string path);
        void Run(string[] args);
    }
}