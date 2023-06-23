using Newtonsoft.Json;
using SharpButtonActionsProj.Service;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpNotesExporter;
using SharpRepoBackendProg.Repetition;
using SharpRepoServiceProg.Service;
using SharpRepoSystemBackendProj;
using TextHeaderAnalyzerCoreProj.Service;
using SharpGoogleDriveProg.Service;

namespace SharpRepoBackendProg
{
    public class BackendService
    {
        private readonly FileService fileService;
        private readonly GoogleDriveService driveService;
        private readonly RepoService repoService;
        private readonly HeaderNotesService headerNotesService;
        private readonly ConfigService configService;

        public BackendService()
        {
            fileService = new FileService();
            configService = new ConfigService(fileService);
            driveService = Border.NewGoogleDriveService();
            configService.PrepareOnlyRepoRootPaths();
            repoService = new RepoService(fileService, configService.GetRepoRootPaths());
            headerNotesService = new HeaderNotesService();
        }

        public void Run(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var buttonActionService = new ButtonActionsService();
            var notesExporterService = new NotesExporterService(repoService);

            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
                {
                    policy.WithOrigins("http://localhost:8081")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseCors(MyAllowSpecificOrigins);
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapGet("/repoApi/{repo}/{loca}", (string repo, string loca) =>
            {
                var loca2 = loca.Replace("-", "/");
                try
                {
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
            });

            app.MapGet("/commandApi/{cmdName}/{repo}/{loca}",
                (string cmdName, string repo, string loca) =>
                {
                    var loca2 = loca.Replace("-", "/");
                    var itemPath = repoService.Methods.GetElemPath((repo, loca2));

                    if (cmdName == "OpenFolder")
                    {
                        buttonActionService.OpenFolder(itemPath);

                        var url = "https://docs.google.com/document/d/18H_5aGqmrch7M_WCJ49PcA0doRxbLCC_bmULwraspe4";
                        var result2 = new Dictionary<string, string> { { "url", url } };
                        var json = JsonConvert.SerializeObject(result2);
                        return json;
                    }

                    if (cmdName == "OpenTextFile")
                    {
                        buttonActionService.OpenTextFile(itemPath);
                    }

                    if (cmdName == "OpenConfigFile")
                    {
                        buttonActionService.OpenConfigFile(itemPath);
                    }

                    if (cmdName == "OpenPdfFile")
                    {
                        var pdfService = new PdfService.PdfService.PdfExecutor2();
                        var textLines = repoService.Methods.ReadTextLines((repo, loca2));
                        var elementsList = headerNotesService.GetElements2(textLines.Skip(4).ToArray());
                        var pdfFilePath = itemPath + "/" + "lista.pdf";
                        var pdfCreated = pdfService.Export(elementsList, pdfFilePath);
                    }

                    if (cmdName == "OpenGoogleDoc")
                    {
                        var name = repoService.Methods.GetLocalName((repo, loca2));
                        var docIdQName = CreateNewDocFile(name);

                        notesExporterService.ExportNotesToGoogleDoc(
                            repo, loca2, docIdQName.id);

                        var url = $"https://docs.google.com/document/d/{docIdQName.id}";
                        var result = new Dictionary<string, string> { { "url", url } };
                        var jsonResult = JsonConvert.SerializeObject(result);
                        return jsonResult;
                    }

                    return JsonConvert.SerializeObject("completed!");
                });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.Run();
        }

        private (string id, string name) CreateNewDocFile(string fileName)
        {
            var folder2023 = "13gY7OdaPCMwHQKmJZWZpcou7xtMrxNlg";
            var result = driveService.Worker.CreateNewDocFile(folder2023, fileName);
            return result;
        }
    }
}
