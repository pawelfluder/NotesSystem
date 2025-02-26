using SharpConfigProg.AAPublic;
using SharpContainerProg.AAPublic;
using SharpFileServiceProg.AAPublic;
using SharpGoogleDocsProg.AAPublic;
using SharpGoogleDriveProg.AAPublic;
using SharpGoogleSheetProg.AAPublic;
using SharpImageSplitterProg.Service;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SharpRepoBackendProg.Services;
using SharpRepoServiceProg.AAPublic;
using SharpTtsServiceProg.AAPublic;
using SharpVideoServiceProg.AAPublic;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using OutBorder1 = SharpOperationsProg.AAPublic.OutBorder;
using OutBorder2 = SharpConfigProg.AAPublic.OutBorder;
using OutBorder3 = SharpRepoServiceProg.AAPublic.OutBorder;
using OutBorder4 = SharpGoogleDriveProg.AAPublic.OutBorder;
using OutBorder5 = SharpGoogleDocsProg.AAPublic.OutBorder;
using OutBorder6 = SharpGoogleSheetProg.AAPublic.OutBorder;
using OutBorder7 = SharpTtsServiceProg.AAPublic.OutBorder;
using OutBorder8 = SharpVideoServiceProg.AAPublic.OutBorder;
using OutBorder12 = SharpRepoBackendProg.AAPublic.OutBorder;
using OutBorder13 = SharpImageSplitterProg.AAPublic.OutBorder;

namespace SharpSetup01Prog.Registrations;

public class DefaultRegistration : RegistrationBase
{
    public Dictionary<string, object> SettingsDict { get; set; }
    public IFileService FileService { get; set; }
    public IOperationsService OperationsService { get; set; }
    private IConfigService ConfigService { get; set; }
    public override void Registrations()
    {
        OutContainer.RegisterByFunc<IFileService>(() => 
            FileService);
        
        OutContainer.RegisterByFunc<IOperationsService>(() => 
            OperationsService);
        
        ConfigService = OutBorder2.ConfigService(OperationsService);
        OutContainer.RegisterByFunc<IConfigService>(
            () => ConfigService);
        
        OutContainer.RegisterByFunc<IFileService, IRepoService>(
            x => OutBorder3.RepoService(x),
            () => OutContainer.Resolve<IFileService>(),
            0,
            InitGroupsFromSearchPaths);
        
        OutContainer.RegisterByFunc<IFileService, IRepoService, IRepoOperationsService>( 
            (x, y) => OutBorder1.RepoOperationsService(x,y),
            () => OutContainer.Resolve<IFileService>(),
            () => OutContainer.Resolve<IRepoService>());
        
        OutContainer.RegisterByFunc<IGoogleDriveService>(
            () => OutBorder4.GoogleDriveService(
                ConfigService.SettingsDict,
                OutContainer.Resolve<IOperationsService>()));
        
        OutContainer.RegisterByFunc<IGoogleDocsService>(
            () => OutBorder5.GoogleDocsService(
                ConfigService.SettingsDict));

        IGoogleSheetService sheetService = OutBorder6
            .GoogleSheetService(ConfigService.SettingsDict);
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
        
        ITtsService ttsService = OutBorder7.TtsService(
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
        IRepoService repoService = OutContainer.Resolve<IRepoService>();
        ConfigService.Prepare(SettingsDict);
        List<string> searchPaths = ConfigService.GetRepoSearchPaths();
        repoService.InitGroupsFromSearchPaths(searchPaths);
    }
}
