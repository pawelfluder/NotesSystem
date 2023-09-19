namespace SharpRepoBackendProg.Service
{
    public partial interface IBackendService
    {
        public enum ApiMethods
        {
            OpenFolder,
            OpenContent,
            OpenConfig,
            OpenPdf,
            OpenGoogledoc,
            CreateGoogledoc,
            CreatePdf,
            RunPrinter,
            GetAllRepoName,
        }
    }
}