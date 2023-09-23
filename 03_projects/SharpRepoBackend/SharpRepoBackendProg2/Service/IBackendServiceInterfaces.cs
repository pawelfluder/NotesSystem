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

            CreateFolder,
            CreateContent,
            CreatePdf,
            CreateGoogledoc,

            AddContent,

            RunPrinter,
            GetAllRepoName,
        }
    }
}