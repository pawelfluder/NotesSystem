using Newtonsoft.Json;
using SharpButtonActionsProj.Service;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpNotesExporter;
using TextHeaderAnalyzerCoreProj.Service;
using SharpRepoBackendProg.Repetition;
using PdfService.PdfService;
using System.Diagnostics;
using SharpRepoServiceProg.AAPublic;
using SharpRepoServiceProg.RepoOperations;
using System.Text.Json.Nodes;
using SharpGoogleDriveProg.AAPublic;
using SharpGoogleDocsProg.AAPublic;

namespace SharpRepoBackendProg.Service
{
    public class BackendService : IBackendService
    {
        // services
        private readonly IFileService fileService;
        private readonly IPdfService2 pdfService;
        private readonly IGoogleDriveService driveService;
        private readonly IGoogleDocsService docsService;
        private readonly IRepoService repoService;
        private readonly IConfigService configService;
        private readonly HeaderNotesService headerNotesService;
        private readonly SystemActionsService buttonActionService;
        private readonly NotesExporterService notesExporterService;

        public BackendService()
        {
            fileService = MyBorder.Container.Resolve<IFileService>();
            pdfService = MyBorder.Container.Resolve<IPdfService2>();
            configService = MyBorder.Container.Resolve<IConfigService>();
            driveService = MyBorder.Container.Resolve<IGoogleDriveService>();
            docsService = MyBorder.Container.Resolve<IGoogleDocsService>();
            repoService = MyBorder.Container.Resolve<IRepoService>();
            headerNotesService = new HeaderNotesService();
            buttonActionService = new SystemActionsService(fileService);
            notesExporterService = new NotesExporterService(repoService);
        }

        public string RepoApi(string repo, string loca)
        {
            var loca2 = loca.Replace("-", "/");
            try
            {
                var address = (repo, loca2);
                var item = repoService.Methods.GetItem(address);
                return item;
            }
            catch
            {
                var error = "Exception! Item not found";var result = new JsonObject();
                result.Add("error", error);
                return result.ToJsonString();
            }
        }

        public string CommandApi(string cmdName, params string[] args)
        {
            string repo = "";
            string loca = "";

            if (args.Length >= 2)
            {
                repo = args[0];
                loca = args[1];
            }

            try
            {
                // config details (ex. names)
                if (cmdName == IBackendService.ApiMethods.GetAllRepoName.ToString())
                {
                    var allRepoNames = repoService.Methods.GetAllReposNames();
                    var jsonString = JsonConvert.SerializeObject(allRepoNames);
                    return jsonString;
                }

                var loca2 = loca.Replace("-", "/");
                var itemPath = repoService.Methods.GetElemPath((repo, loca2));

                // var address = (repo, loca);
                // if (cmdName == IBackendService.ApiMethods.GetName.ToString())
                // {
                //     var item = repoService.Methods.GetItem(address);
                //     var tmp = Json.Deserialize(item);
                //     var name = tmp["name"];
                //     return Json.Serialize(name)
                // }
                

                // folder
                if (cmdName == IBackendService.ApiMethods.OpenFolder.ToString())
                {
                    buttonActionService.OpenFolder(itemPath);
                    var json = GetOKJson();
                    return json;
                }
                
                //content
                if (cmdName == IBackendService.ApiMethods.OpenContent.ToString())
                {
                    var path = itemPath + "/" + "lista.txt";
                    buttonActionService.OpenFile(path);
                    var json = GetOKJson();
                    return json;
                }
                // if (cmdName == IBackendService.ApiMethods.GetContent.ToString())
                // {
                //     var item = repoService.Methods.GetItem(address);
                //     var body = item["Body"];
                //     return body;
                // }

                // item
                var address = (repo, loca);
                if (cmdName == IBackendService.ApiMethods.GetItem.ToString())
                {
                    var item = repoService.Methods.GetItem(address);
                    return item;
                }
                if (cmdName == IBackendService.ApiMethods.CreateItem.ToString())
                {
                    var type = args[2];
                    var name = args[3];
                    var item = repoService.Methods.CreateItem(address, type, name);
                    return item;
                }

                // config
                if (cmdName == IBackendService.ApiMethods.OpenConfig.ToString())
                {
                    // todo join name of file
                    var path = itemPath + "/" + "nazwa.txt";
                    buttonActionService.OpenFile(path);
                }

                // pdf
                if (cmdName == IBackendService.ApiMethods.CreatePdf.ToString())
                {
                    CreatePdf((repo, loca2));
                }
                if (cmdName == IBackendService.ApiMethods.OpenPdf.ToString())
                {
                    var pdfPath = CreatePdf((repo, loca2));
                    var success = pdfService.Open(pdfPath);
                    return success.ToString();
                }

                // terminal
                if (cmdName == IBackendService.ApiMethods.OpenTerminal.ToString())
                {
                    buttonActionService.OpenTerminal(itemPath);
                }

                // google docs
                if (cmdName == IBackendService.ApiMethods.OpenGoogleDoc.ToString())
                {
                    var url = OpenGoogledoc((repo, loca2));
                    OpenGoogledoc(url);
                }
                if (cmdName == IBackendService.ApiMethods.CreateGoogleDoc.ToString())
                {
                    var url = CreateGoogledoc((repo, loca2));
                    var result = new Dictionary<string, string> { { "url", url } };
                    var jsonResult = JsonConvert.SerializeObject(result);
                    return jsonResult;
                }
                if (cmdName == IBackendService.ApiMethods.RecreateGoogleDoc.ToString())
                {
                    var url = CreateGoogledoc((repo, loca2));
                    OpenGoogledoc(url);
                }

                // printer
                if (cmdName == IBackendService.ApiMethods.RunPrinter.ToString())
                {
                    //var pdfPath = CreatePdf((repo, loca2));
                    var pdfPath = itemPath + "/" + "lista.pdf";
                    pdfService.RunPrinter(pdfPath);
                    return string.Empty;
                }
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject("bad request - exception occured!");
            }

            return JsonConvert.SerializeObject("bad request - method not found!");
        }

        private string GetOKJson()
        {
            var result = "200; OK";
            var json = JsonConvert.SerializeObject(result);
            return json;
        }

        // public string CommandApi(string cmdName, string[] args)
        // {
        //     try
        //     {
        //         // zero arguments
        //         if (cmdName == IBackendService.ApiMethods.GetAllRepoName.ToString())
        //         {
        //             var allRepoNames = repoService.Methods.GetAllReposNames();
        //             var jsonString = JsonConvert.SerializeObject(allRepoNames);
        //             return jsonString;
        //         }

        //         // two arguments
        //         var repo = args[0];
        //         var loca = args[1].Replace("-", "/");
        //         var address = (repo, loca);
        //         var itemPath = repoService.Methods.GetElemPath((repo, loca));

        //         if (cmdName == IBackendService.ApiMethods.OpenFolder.ToString())
        //         {
        //             buttonActionService.OpenFolder(itemPath);

        //             var url = "https://docs.google.com/document/d/18H_5aGqmrch7M_WCJ49PcA0doRxbLCC_bmULwraspe4";
        //             var result2 = new Dictionary<string, string> { { "url", url } };
        //             var json = JsonConvert.SerializeObject(result2);
        //             return json;
        //         }

        //         if (cmdName == IBackendService.ApiMethods.OpenContent.ToString())
        //         {
        //             buttonActionService.OpenContent(itemPath);
        //         }

        //         if (cmdName == IBackendService.ApiMethods.OpenConfig.ToString())
        //         {
        //             buttonActionService.OpenConfigFile(itemPath);
        //         }

        //         if (cmdName == IBackendService.ApiMethods.CreatePdf.ToString())
        //         {
        //             CreatePdf((repo, loca));
        //         }

        //         if (cmdName == IBackendService.ApiMethods.OpenPdf.ToString())
        //         {
        //             var pdfPath = CreatePdf((repo, loca));
        //             var success = pdfService.Open(pdfPath);
        //             return success.ToString();
        //         }

        //         if (cmdName == IBackendService.ApiMethods.CreateGoogleDoc.ToString())
        //         {
        //             var url = CreateGoogledoc((repo, loca));
        //             var result = new Dictionary<string, string> { { "url", url } };
        //             var jsonResult = JsonConvert.SerializeObject(result);
        //             return jsonResult;
        //         }

        //         if (cmdName == IBackendService.ApiMethods.OpenGoogleDoc.ToString())
        //         {
        //             var url = CreateGoogledoc((repo, loca));
        //             OpenGoogledoc(url);
        //         }

        //         if (cmdName == IBackendService.ApiMethods.RunPrinter.ToString())
        //         {
        //             //var pdfPath = CreatePdf((repo, loca2));
        //             var pdfPath = itemPath + "/" + "lista.pdf";
        //             pdfService.RunPrinter(pdfPath);
        //             return string.Empty;
        //         }

        //         if (cmdName == IBackendService.ApiMethods.CreateFolder.ToString())
        //         {
        //             var type = args[2];
        //             var name = args[3];
        //             if (type == "Folder")
        //             {
        //                 var outputItem = repoService.Methods
        //                     .CreateChildFolder(address, name);
        //                 return JsonConvert.SerializeObject("completed!");
        //             }
        //             if (type == "Text")
        //             {
        //                 var outputItem = repoService.Methods
        //                     .CreateChildText(address, name);
        //                 return JsonConvert.SerializeObject("completed!");
        //             }
        //         }

        //         if (cmdName == IBackendService.ApiMethods.AddContent.ToString())
        //         {
        //             var content = args[2];
        //             repoService.Methods
        //                     .AppendText(address, content);
        //             return JsonConvert.SerializeObject("completed!");
        //         }
        //     }
        //     catch { }

        //     return JsonConvert.SerializeObject("completed!");
        // }

        private void OpenGoogledoc(string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private string OpenGoogledoc((string repo, string loca) address)
        {
            var name = repoService.Methods.GetLocalName(address);
            var id = repoService.Methods.TryGetConfigValue(
                address, ConfigKeys.googleDocId.ToString());
            var documentExists = id != null;

            if (!documentExists)
            {
                var docIdQName = CreateNewDocFile(name);
                id = docIdQName.id;
                repoService.Methods.CreateConfigKey(
                    address, ConfigKeys.googleDocId.ToString(),
                    id);
                documentExists = true;
            }

            if (documentExists)
            {
                var url = $"https://docs.google.com/document/d/{id}";
                return url;
            }

            return default;
        }

        private string CreateGoogledoc((string repo, string loca) address)
        {
            var name = repoService.Methods.GetLocalName(address);
            var id = repoService.Methods.TryGetConfigValue(
                address, ConfigKeys.googleDocId.ToString());
            var documentExists = id != null;

            if (!documentExists)
            {
                var docIdQName = CreateNewDocFile(name);
                id = docIdQName.id;
                repoService.Methods.CreateConfigKey(
                    address, ConfigKeys.googleDocId.ToString(),
                    id);
                documentExists = true;
            }

            if (documentExists)
            {
                notesExporterService.ExportNotesToGoogleDoc(
                    address.repo, address.loca, id.ToString());

                var url = $"https://docs.google.com/document/d/{id}";
                return url;
            }

            return default;
        }

        private string CreatePdf((string Repo, string Loca) address)
        {
            var itemPath = repoService.Methods.GetElemPath(address);
            var pdfService = MyBorder.Container.Resolve<IPdfService2>();
            var textLines = repoService.Methods.GetTextLines((address.Repo, address.Loca));
            var elementsList = headerNotesService.GetElements2(textLines.Skip(4).ToArray());
            var pdfFilePath = itemPath + "/" + "lista.pdf";
            var pdfCreated = pdfService.Export(elementsList, pdfFilePath);
            return pdfFilePath;
        }

        private (string id, string name) CreateNewDocFile(string fileName)
        {
            // Opcja druga za pomocą google drive
            // var nameQId = driveService.Worker.CreateNewDocFile(null, fileName);

            var document = docsService.StackWkr.CreateDocFile(fileName);
            var permission = driveService.Worker.AddReadPermissionForAnyone(document.DocumentId);
            var result = (document.DocumentId, document.Title);            
            return result;
        }

        private (string id, string name) SetDocFileName(string fileName)
        {
            // todo
            return (default, default);
        }

        public string RepoApi(string methodName, params string[] args)
        {
            try
            {
                var methodList = typeof(RepoWorker).GetMethods().Where(x => x.Name == methodName);
                var method = methodList.SingleOrDefault(x => x.GetParameters().Length == args.Length);

                var result = method.Invoke(
                    repoService.Methods,
                    args);
                return result.ToString();
            }
            catch { }

            return default;
        }
    }
}
