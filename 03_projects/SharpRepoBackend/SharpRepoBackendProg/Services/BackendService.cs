using System.Diagnostics;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using PdfService.PdfService;
using SharpApiArgsProg.AAPublic;
using SharpButtonActionsProg.Services;
using SharpConfigProg.AAPublic;
using SharpFileServiceProg.AAPublic;
using SharpGoogleDocsProg.AAPublic;
using SharpGoogleDriveProg.AAPublic;
using SharpNotesExporterProg;
using SharpOperationsProg.AAPublic;
using SharpRepoBackendProg.Repetition;
using SharpRepoServiceProg.AAPublic;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Workers.APublic;
using SharpRepoServiceProg.Workers.APublic.ItemWorkers;
using SharpTtsServiceProg.AAPublic;
using TextHeaderAnalyzerCoreProj.Service;
//#pragma warning disable CS8618

namespace SharpRepoBackendProg.Services;

public class BackendService : IBackendService
{
    // MODULES SERVICES
    private readonly IPdfService2 _pdfService;
    private readonly IGoogleDriveService _driveService;
    private readonly IGoogleDocsService _docsService;
    private readonly IRepoService _repoService;
    private readonly IConfigService _configService;
    private readonly HeaderNotesService _headerNotesService;
    private readonly MainButtonActionsService buttonActionActionService;
    private readonly NotesExporterService _notesExporterService;
    private readonly IFileService _fileService;
    private readonly ITtsService _ttsService;
    private readonly IStringArgsResolverService _stringArgs;

    public BackendService()
    {
        IOperationsService operationsService = MyBorder.OutContainer.Resolve<IOperationsService>();
        _fileService = operationsService.GetFileService();
        _ttsService = MyBorder.OutContainer.Resolve<ITtsService>();
        _pdfService = MyBorder.OutContainer.Resolve<IPdfService2>();
        _configService = MyBorder.OutContainer.Resolve<IConfigService>();
        _driveService = MyBorder.OutContainer.Resolve<IGoogleDriveService>();
        _docsService = MyBorder.OutContainer.Resolve<IGoogleDocsService>();
        _repoService = MyBorder.OutContainer.Resolve<IRepoService>();
        _headerNotesService = new HeaderNotesService();
        buttonActionActionService = new MainButtonActionsService(operationsService, _repoService);
        _notesExporterService = new NotesExporterService(_repoService);
        
        _stringArgs = MyBorder.OutContainer.Resolve<IStringArgsResolverService>();
    }

    public string RepoApi(string repo, string inputLoca)
    {
        string loca = inputLoca.Replace("-", "/");
        try
        {
            string serviceName = nameof(IRepoService);
            string workerName = nameof(IItemWorker);
            string methodName = nameof(IItemWorker.GetItem);
            string param01 = repo;
            string param02 = loca;
            string itemJson = _stringArgs.Invoke(
            [serviceName, workerName, methodName,
                param01, param02]);
            return itemJson;
        }
        catch
        {
            var error = "Exception! Item not found";
            var result = new JsonObject();
            result.Add("error", error);
            return result.ToJsonString();
        }
    }
    
    public string InvokeStringArgsApi(
        params string[] args)
    {
        string itemJson = _stringArgs.Invoke(args);
        return itemJson;
    }

    // public string CommandApi2(
    //     string cmdName,
    //     params string[] args)
    // {
    //     string repo = "";
    //     string loca = "";
    //
    //     if (args.Length >= 2)
    //     {
    //         repo = args[0];
    //         loca = args[1];
    //     }
    //
    //     try
    //     {
    //         // config details (ex. names)
    //         if (cmdName == ApiMethods.GetAllRepoName.ToString())
    //         {
    //             var allRepoNames = _repoService.Methods.GetAllReposNames();
    //             var jsonString = JsonConvert.SerializeObject(allRepoNames);
    //             return jsonString;
    //         }
    //
    //         var loca2 = loca.Replace("-", "/");
    //         var itemPath = _repoService.Methods.GetElemPath((repo, loca2));
    //
    //         // var address = (repo, loca);
    //         // if (cmdName == IBackendService.ApiMethods.GetName.ToString())
    //         // {
    //         //     var item = repoService.Methods.GetItem(address);
    //         //     var tmp = Json.Deserialize(item);
    //         //     var name = tmp["name"];
    //         //     return Json.Serialize(name)
    //         // }
    //             
    //
    //         // folder
    //         if (cmdName == ApiMethods.OpenFolder.ToString())
    //         {
    //             _buttonActionService.OpenFolder(itemPath);
    //             var json = GetOKJson();
    //             return json;
    //         }
    //             
    //         //content
    //         if (cmdName == ApiMethods.OpenContent.ToString())
    //         {
    //             var path = itemPath + "/" + "lista.txt";
    //             _buttonActionService.OpenFile(path);
    //             var json = GetOKJson();
    //             return json;
    //         }
    //         // if (cmdName == ApiMethods.GetContent.ToString())
    //         // {
    //         //     var item = repoService.Methods.GetItem(address);
    //         //     var body = item["Body"];
    //         //     return body;
    //         // }
    //
    //         // item
    //         var address = (Repo: repo, Loca: loca);
    //         if (cmdName == ApiMethods.Tts.ToString())
    //         {
    //             object builder = _ttsService.Tts
    //                 .GetBuilder(address, new CultureInfo("pl-PL"));
    //             _ttsService.RepoTts.PlStartNew(builder);
    //             var json = GetOKJson();
    //             return json;
    //         }
    //         if (cmdName == ApiMethods.GetItem.ToString())
    //         {
    //             var item = _repoService.Methods.GetItem(address);
    //             return item;
    //         }
    //         if (cmdName == ApiMethods.CreateItem.ToString())
    //         {
    //             var type = args[2];
    //             var name = args[3];
    //
    //             var item = _repoService.Item.PostParentItem(address, type, name);
    //             return item;
    //         }
    //
    //         // config
    //         if (cmdName == ApiMethods.OpenConfig.ToString())
    //         {
    //             // todo join name of file
    //             var path = itemPath + "/" + "nazwa.txt";
    //             _buttonActionService.OpenFile(path);
    //         }
    //
    //         // pdf
    //         if (cmdName == ApiMethods.CreatePdf.ToString())
    //         {
    //             CreatePdf((repo, loca2));
    //         }
    //         if (cmdName == ApiMethods.OpenPdf.ToString())
    //         {
    //             var pdfPath = CreatePdf((repo, loca2));
    //             var success = _pdfService.Open(pdfPath);
    //             return success.ToString();
    //         }
    //
    //         // terminal
    //         if (cmdName == ApiMethods.OpenTerminal.ToString())
    //         {
    //             _buttonActionService.OpenTerminal(itemPath);
    //         }
    //
    //         // google docs
    //         if (cmdName == ApiMethods.OpenGoogleDoc.ToString())
    //         {
    //             var url = OpenGoogledoc((repo, loca2));
    //             OpenGoogledoc(url);
    //         }
    //         if (cmdName == ApiMethods.CreateGoogleDoc.ToString())
    //         {
    //             var url = CreateGoogledoc((repo, loca2));
    //             var result = new Dictionary<string, string> { { "url", url } };
    //             var jsonResult = JsonConvert.SerializeObject(result);
    //             return jsonResult;
    //         }
    //         if (cmdName == ApiMethods.RecreateGoogleDoc.ToString())
    //         {
    //             var url = CreateGoogledoc((repo, loca2));
    //             OpenGoogledoc(url);
    //         }
    //
    //         // printer
    //         if (cmdName == ApiMethods.RunPrinter.ToString())
    //         {
    //             //var pdfPath = CreatePdf((repo, loca2));
    //             var pdfPath = itemPath + "/" + "lista.pdf";
    //             _pdfService.RunPrinter(pdfPath);
    //             return string.Empty;
    //         }
    //     }
    //     catch(Exception ex)
    //     {
    //         return JsonConvert.SerializeObject("bad request - exception occured!");
    //     }
    //
    //     return JsonConvert.SerializeObject("bad request - method not found!");
    // }

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
    //         if (cmdName == ApiMethods.GetAllRepoName.ToString())
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

    //         if (cmdName == ApiMethods.OpenFolder.ToString())
    //         {
    //             buttonActionService.OpenFolder(itemPath);

    //             var url = "https://docs.google.com/document/d/18H_5aGqmrch7M_WCJ49PcA0doRxbLCC_bmULwraspe4";
    //             var result2 = new Dictionary<string, string> { { "url", url } };
    //             var json = JsonConvert.SerializeObject(result2);
    //             return json;
    //         }

    //         if (cmdName == ApiMethods.OpenContent.ToString())
    //         {
    //             buttonActionService.OpenContent(itemPath);
    //         }

    //         if (cmdName == ApiMethods.OpenConfig.ToString())
    //         {
    //             buttonActionService.OpenConfigFile(itemPath);
    //         }

    //         if (cmdName == ApiMethods.CreatePdf.ToString())
    //         {
    //             CreatePdf((repo, loca));
    //         }

    //         if (cmdName == ApiMethods.OpenPdf.ToString())
    //         {
    //             var pdfPath = CreatePdf((repo, loca));
    //             var success = pdfService.Open(pdfPath);
    //             return success.ToString();
    //         }

    //         if (cmdName == ApiMethods.CreateGoogleDoc.ToString())
    //         {
    //             var url = CreateGoogledoc((repo, loca));
    //             var result = new Dictionary<string, string> { { "url", url } };
    //             var jsonResult = JsonConvert.SerializeObject(result);
    //             return jsonResult;
    //         }

    //         if (cmdName == ApiMethods.OpenGoogleDoc.ToString())
    //         {
    //             var url = CreateGoogledoc((repo, loca));
    //             OpenGoogledoc(url);
    //         }

    //         if (cmdName == ApiMethods.RunPrinter.ToString())
    //         {
    //             //var pdfPath = CreatePdf((repo, loca2));
    //             var pdfPath = itemPath + "/" + "lista.pdf";
    //             pdfService.RunPrinter(pdfPath);
    //             return string.Empty;
    //         }

    //         if (cmdName == ApiMethods.CreateFolder.ToString())
    //         {
    //             var type = args[2];
    //             var name = args[3];
    //             if (type == "Folder")
    //             {
    //                 var outputItem = repoService.Methods
    //                     .CreateChildFolder(address, name);
    //                 return JsonConvert.SerializeObject("completed!");
    //             }
    //             if (type == ItemTypes.Text)
    //             {
    //                 var outputItem = repoService.Methods
    //                     .CreateChildText(address, name);
    //                 return JsonConvert.SerializeObject("completed!");
    //             }
    //         }

    //         if (cmdName == ApiMethods.AddContent.ToString())
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
        var name = _repoService.Methods.GetLocalName(address);
        var id = _repoService.Methods.TryGetConfigValue(
            address, ConfigKeys.GoogleDocId.ToString());
        var documentExists = id != null;

        if (!documentExists)
        {
            var docIdQName = CreateNewDocBySheetService(name);
            id = docIdQName.id;
            _repoService.Methods.CreateConfigKey(
                address, ConfigKeys.GoogleDocId.ToString(),
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

    private string CreateGoogledoc(
        (string repo, string loca) adrTuple)
    {
        var name = _repoService.Methods.GetLocalName(adrTuple);
        var id = _repoService.Methods.GetConfigKey(
            adrTuple, ConfigKeys.GoogleDocId.ToString());
        var documentExists = id != null;

        if (!documentExists)
        {
            //var docIdQName = CreateNewDocBySheetService(name);
            var docIdQName = CreateNewDocByDriveService(name);

            id = docIdQName.id;
            _repoService.Methods.CreateConfigKey(
                adrTuple, ConfigKeys.GoogleDocId.ToString(),
                id);
            documentExists = true;
        }

        if (documentExists)
        {
            _notesExporterService.ExportNotesToGoogleDoc(
                adrTuple.repo, adrTuple.loca, id.ToString());

            var url = $"https://docs.google.com/document/d/{id}";
            return url;
        }

        return default;
    }

    private string CreatePdf((string Repo, string Loca) address)
    {
        var itemPath = _repoService.Methods.GetElemPath(address);
        var pdfService = MyBorder.OutContainer.Resolve<IPdfService2>();
        var textLines = _repoService.Methods.GetTextLines((address.Repo, address.Loca));
        var elementsList = _headerNotesService.GetElements2(textLines.ToArray());
        var pdfFilePath = itemPath + "/" + "lista.pdf";
        var pdfCreated = pdfService.Export(elementsList, pdfFilePath);
        return pdfFilePath;
    }

    private (string id, string name) CreateNewDocBySheetService(string title)
    {
        var document = _docsService.Stack.CreateDocFile(title);
        var permission = _driveService.Composite.AddReadPermissionForAnyone(document.DocumentId);
        var docIdQName = (document.DocumentId, document.Title);            
        return docIdQName;
    }

    private (string id, string name) CreateNewDocByDriveService(string title)
    {
        var docId = _driveService.Composite.CreateDocFile(title);
        var permission = _driveService.Composite.AddReadPermissionForAnyone(docId);
        var docIdQName = (docId, title);
        return docIdQName;
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
            var methodList = typeof(MethodWorker).GetMethods().Where(x => x.Name == methodName);
            var method = methodList.SingleOrDefault(x => x.GetParameters().Length == args.Length);

            var result = method.Invoke(
                _repoService.Methods,
                args);
            return result.ToString();
        }
        catch { }

        return default;
    }
}