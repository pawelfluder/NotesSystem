using Microsoft.Extensions.DependencyInjection;
using SharpConfigProg.AAPublic;
using SharpContainerProg.AAPublic;
using SharpContainerProg.Containers;
using SharpFileServiceProg.AAPublic;
using SharpGoogleDocsProg.AAPublic;
using SharpGoogleDriveProg.AAPublic;
using SharpGoogleSheetProg.AAPublic;
using SharpImageSplitterProg.Service;
using SharpOperationsProg.AAPublic.Operations;
using SharpRepoBackendProg.Services;
using SharpRepoServiceProg.AAPublic;
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
    private Dictionary<string, object> _settingsDict;

    public void SetSettingsDict(
        Dictionary<string, object> inputDict)
    {
        _settingsDict = inputDict;
    }

    public override void Registrations()
    {
        OutContainer.RegisterByFunc<IFileService>(() => 
            OutBorder1.FileService());
        
        OutContainer.RegisterByFunc<IFileService, IOperationsService>(
            x => OutBorder9.OperationsService(x),
            () => OutContainer.Resolve<IFileService>());
        
        OutContainer.RegisterByFunc<IOperationsService, IConfigService>(
            x => OutBorder2.ConfigService(x),
            () => OutContainer.Resolve<IOperationsService>());
        
        OutContainer.RegisterByFunc<IFileService, IRepoService>(
            x => OutBorder3.RepoService(x),
            () => OutContainer.Resolve<IFileService>(),
            0,
            InitGroupsFromSearchPaths);

        IGoogleDriveService googleDrive = OutBorder4.GoogleDriveService(
            _settingsDict,
            OutContainer.Resolve<IOperationsService>());
        OutContainer.RegisterByFunc<IGoogleDriveService>(
            () => googleDrive);

        IGoogleDocsService googleDocs = OutBorder5.GoogleDocsService(
            _settingsDict);
        OutContainer.RegisterByFunc<IGoogleDocsService>(
            () => googleDocs);

        IGoogleSheetService sheetService = OutBorder6.GoogleSheetService(
            _settingsDict);
        OutContainer.RegisterByFunc(() => sheetService);
        
        // var argsManager = OutBorder11.ArgsManagerService();
        // OutContainer.RegisterByFunc<IArgsManagerService>(
        //     () => argsManager);
        
        OutContainer.RegisterByFunc(
            (x) => OutBorder8.VideoService(x),
            () => OutContainer.Resolve<IOperationsService>());
        
        // OutContainer.RegisterByFunc(
        //     (x) => OutBorder7.TtsService(),
        //     () => OutContainer.Resolve<IOperationsService>());
        
        var ttsService = OutBorder7.TtsService(
            OutContainer.Resolve<IOperationsService>(),
            OutContainer.Resolve<IRepoService>(),
            OutContainer.Resolve<IVideoService>());
        OutContainer.RegisterByFunc(() => ttsService);
        OutContainer.ServiceRegister.AddSpeechSynthesis();

        IBackendService backend = OutBorder12.BackendService();
        OutContainer.RegisterByFunc(() => backend);

        IImageSpliterService imageSpliter = OutBorder13.ImageSpliterService();
        OutContainer.RegisterByFunc(() => imageSpliter);
        
        // OutContainer.ServiceRegister.AddSpeechSynthesisServices();
        OutContainer.ServiceRegister.AddSpeechSynthesis();
    }

    private void InitGroupsFromSearchPaths()
    {
        IConfigService configService = OutContainer.Resolve<IConfigService>();
        IRepoService repoService = OutContainer.Resolve<IRepoService>();
        configService.Prepare(_settingsDict);
        List<string> searchPaths = configService.GetRepoSearchPaths();
        repoService.InitGroupsFromSearchPaths(searchPaths);
    }
}
