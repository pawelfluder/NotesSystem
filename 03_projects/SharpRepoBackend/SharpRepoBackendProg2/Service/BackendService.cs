using Newtonsoft.Json;
using SharpButtonActionsProj.Service;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpNotesExporter;
using SharpRepoServiceProg.Service;
using SharpRepoSystemBackendProj;
using TextHeaderAnalyzerCoreProj.Service;
using SharpGoogleDriveProg.Service;
using SharpConfigProg.Preparer;
using Unity;
using SharpRepoBackendProg.Repetition;
using PdfService.PdfService;
using System.Diagnostics;
using static SharpRepoServiceProg.Service.IRepoService;

namespace SharpRepoBackendProg.Service
{
    internal class BackendService : IBackendService
    {
        private readonly IFileService fileService;
        private readonly IPdfService2 pdfService;
        private readonly GoogleDriveService driveService;
        private readonly RepoService repoService;
        private readonly HeaderNotesService headerNotesService;
        private readonly ButtonActionsService buttonActionService;
        private readonly NotesExporterService notesExporterService;
        private readonly IConfigService configService;

        public BackendService()
        {
            fileService = MyBorder.Container.Resolve<IFileService>();
            pdfService = MyBorder.Container.Resolve<IPdfService2>();
            configService = MyBorder.Container.Resolve<IConfigService>();
            driveService = MyBorder.NewGoogleDriveService();
            configService.Prepare(typeof(IPreparer.INotesSystem));
            repoService = new RepoService(fileService, configService.GetRepoSearchPaths());
            headerNotesService = new HeaderNotesService();
            buttonActionService = new ButtonActionsService();
            notesExporterService = new NotesExporterService(repoService);
        }

        public string RepoApi(string repo, string loca)
        {
            var loca2 = loca.Replace("-", "/");
            try
            {
                var repoRootPaths = repoService.Methods.GetAllReposPaths();
                var name = repoService.Methods.GetLocalName((repo, loca2));
                var content = repoService.Methods.ReadTextLines((repo, loca2));
                var data = new Data(name, content);
                var json = JsonConvert.SerializeObject(data);
                return json;
            }
            catch
            {
                var error = "Exception! Item not found";
                var result = new Dictionary<string, string> { { "error", error } };
                var jsonResult = JsonConvert.SerializeObject(result);
                return jsonResult;
            }
        }

        public string CommandApi(string cmdName, string repo, string loca)
        {
            var loca2 = loca.Replace("-", "/");
            var itemPath = repoService.Methods.GetElemPath((repo, loca2));

            if (cmdName == IBackendService.ApiMethods.OpenFolder.ToString())
            {
                buttonActionService.OpenFolder(itemPath);

                var url = "https://docs.google.com/document/d/18H_5aGqmrch7M_WCJ49PcA0doRxbLCC_bmULwraspe4";
                var result2 = new Dictionary<string, string> { { "url", url } };
                var json = JsonConvert.SerializeObject(result2);
                return json;
            }

            if (cmdName == IBackendService.ApiMethods.OpenContent.ToString())
            {
                buttonActionService.OpenContent(itemPath);
            }

            if (cmdName == IBackendService.ApiMethods.OpenConfig.ToString())
            {
                buttonActionService.OpenConfigFile(itemPath);
            }

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

            if (cmdName == IBackendService.ApiMethods.CreateGoogledoc.ToString())
            {
                var url = CreateGoogledoc((repo, loca2));
                var result = new Dictionary<string, string> { { "url", url } };
                var jsonResult = JsonConvert.SerializeObject(result);
                return jsonResult;
            }

            if (cmdName == IBackendService.ApiMethods.OpenGoogledoc.ToString())
            {
                var url = CreateGoogledoc((repo, loca2));
                OpenGoogledoc(url);
            }

            if (cmdName == IBackendService.ApiMethods.RunPrinter.ToString())
            {
                //var pdfPath = CreatePdf((repo, loca2));
                var pdfPath = itemPath + "/" + "lista.pdf";
                pdfService.RunPrinter(pdfPath);
                return string.Empty;
            }

            return JsonConvert.SerializeObject("completed!");
        }

        private void OpenGoogledoc(string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private string CreateGoogledoc((string repo, string loca) address)
        {
            var name = repoService.Methods.GetLocalName(address);
            var id = repoService.Methods.GetConfigKeyValue(
                address, ConfigKeys.googleDocId.ToString());
            var documentAlreadyExisted = id != null;

            if (!documentAlreadyExisted)
            {
                var docIdQName = CreateNewDocFile(name);
                id = docIdQName.id;
            }

            if (documentAlreadyExisted)
            {
                // todo
                // notesExporterService.SetDocFileName(name)
            }

            notesExporterService.ExportNotesToGoogleDoc(
                address.repo, address.loca, id.ToString());

            if (!documentAlreadyExisted)
            {
                repoService.Methods.CreateConfigKey(
                    address, ConfigKeys.googleDocId.ToString(),
                    id);
            }

            var url = $"https://docs.google.com/document/d/{id}";
            return url;
        }

        private string CreatePdf((string Repo, string Loca) address)
        {
            var itemPath = repoService.Methods.GetElemPath(address);
            var pdfService = MyBorder.Container.Resolve<IPdfService2>();
            var textLines = repoService.Methods.ReadTextLines((address.Repo, address.Loca));
            var elementsList = headerNotesService.GetElements2(textLines.Skip(4).ToArray());
            var pdfFilePath = itemPath + "/" + "lista.pdf";
            var pdfCreated = pdfService.Export(elementsList, pdfFilePath);
            return pdfFilePath;
        }

        private (string id, string name) CreateNewDocFile(string fileName)
        {
            var folder2023 = "13gY7OdaPCMwHQKmJZWZpcou7xtMrxNlg";
            var result = driveService.Worker.CreateNewDocFile(folder2023, fileName);
            return result;
        }

        private (string id, string name) SetDocFileName(string fileName)
        {
            // todo
            return (default, default);
        }
    }
}
