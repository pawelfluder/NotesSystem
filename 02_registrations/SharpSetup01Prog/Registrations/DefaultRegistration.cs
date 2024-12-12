using Microsoft.Extensions.DependencyInjection;
using SharpContainerProg.AAPublic;
using SharpContainerProg.Containers;
using SharpFileServiceProg.AAPublic;
using SharpImageSplitterProg.Service;
using SharpOperationsProg.AAPublic.Operations;
using SharpRepoBackendProg.Services;
using SharpVideoServiceProg.AAPublic;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using OutBorder1 = SharpFileServiceProg.AAPublic.OutBorder;
using OutBorder2 = SharpConfigProg.AAPublic.OutBorder;
using OutBorder3 = SharpRepoServiceProg.AAPublic.OutBorder;
using OutBorder4 = SharpGoogleDriveProg.AAPublic.OutBorder;
using OutBorder5 = SharpGoogleDocsProg.AAPublic.OutBorder;
using OutBorder6 = SharpGoogleSheetProg.AAPublic.OutBorder;
using OutBorder7 = SharpTtsServiceProg.AAPublic.OutBorder;
using OutBorder8 = SharpVideoServiceProg.AAPublic.OutBorder;
using OutBorder9 = SharpOperationsProg.AAPublic.OutBorder;
using OutBorder12 = SharpRepoBackendProg.AAPublic.OutBorder;
using OutBorder13 = SharpImageSplitterProg.AAPublic.OutBorder;

namespace SharpSetup01Prog.Registrations;

public class DefaultRegistration : RegistrationBase
{
    private Dictionary<string, object> _tempDict;

    public void SetSettingsDict(Dictionary<string, object> inputDict)
    {
        _tempDict = inputDict;
    }

    public override void Registrations()
    {
        IFileService fileService = OutBorder1.FileService();
        OutContainer.RegisterByFunc(() => fileService);
            
        var operationsService = OutBorder9.OperationsService(fileService);
        OutContainer.RegisterByFunc<IOperationsService>(
            () => operationsService);

        var configService = OutBorder2.ConfigService(operationsService);
        OutContainer.RegisterByFunc(() => configService);

        var repoService = OutBorder3.RepoService(fileService);
        OutContainer.RegisterByFunc(() => repoService);

        configService.Prepare(_tempDict);
        var searchPaths = configService.GetRepoSearchPaths();
        repoService.PutPaths(searchPaths);

        var driveService = OutBorder4.GoogleDriveService(
            configService.SettingsDict,
            operationsService);
        OutContainer.RegisterByFunc(() => driveService);

        var docsService = OutBorder5.GoogleDocsService(
            configService.SettingsDict);
        OutContainer.RegisterByFunc(() => docsService);

        var sheetService = OutBorder6.GoogleSheetService(
            configService.SettingsDict);
        OutContainer.RegisterByFunc(() => sheetService);
        
        // var argsManager = OutBorder11.ArgsManagerService();
        // OutContainer.RegisterByFunc<IArgsManagerService>(
        //     () => argsManager);

        IVideoService videoService = OutBorder8
            .VideoService(operationsService);
        OutContainer.RegisterByFunc(() => sheetService);
        
        var ttsService = OutBorder7.TtsService(operationsService, repoService, videoService);
        OutContainer.RegisterByFunc(() => ttsService);
        OutContainer.ServiceRegister.AddSpeechSynthesis();

        IBackendService backend = OutBorder12.BackendService();
        OutContainer.RegisterByFunc(() => backend);

        IImageSpliterService imageSpliter = OutBorder13.ImageSpliterService();
        OutContainer.RegisterByFunc(() => imageSpliter);
        
        // OutContainer.ServiceRegister.AddSpeechSynthesisServices();
        OutContainer.ServiceRegister.AddSpeechSynthesis();
    }
}
