namespace SharpRepoBackendProg.Service
{
    public partial interface IBackendService
    {
        public enum ApiMethods
        {
            // item
            GetItem,
            CreateItem,

            // name
            GetName,

            // body
            GetBody,

            // config
            GetConfig,
            OpenConfig,
            CreateFolder,

            // content
            GetContent,

            OpenContent,
            CreateContent,
            AddContent,

            // folder
            OpenFolder,

            // terminal
            OpenTerminal,

            // pdf
            OpenPdf,
            CreatePdf,

            // google doc
            OpenGoogleDoc,
            RecreateGoogleDoc,
            CreateGoogleDoc,

            // printer
            RunPrinter,

            // ??
            GetAllRepoName,
        }
    }
}