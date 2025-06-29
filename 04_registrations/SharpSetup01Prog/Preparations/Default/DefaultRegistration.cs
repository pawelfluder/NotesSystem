using SharpApiArgsProg.AAPublic;
using SharpButtonActionsProg.AAPublic;
using SharpConfigProg.AAPublic;
using SharpContainerProg.AAPublic;
using SharpFileServiceProg.AAPublic;
using SharpGoogleDocsProg.AAPublic;
using SharpGoogleDriveProg.AAPublic;
using SharpGoogleSheetProg.AAPublic;
using SharpIdentityProg.AAPublic;
using SharpImageSplitterProg.Service;
using SharpOperationsProg.AAPublic;
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
using OutBorder14 = SharpApiArgsProg.AAPublic.OutBorder;
using OutBorder15 = SharpButtonActionsProg.AAPublic.OutBorder;
using OutBorder16 = SharpIdentityProg.AAPublic.OutBorder;

namespace SharpSetup01Prog.Preparations.Default;

public class DefaultRegistration : RegistrationBase
{
    public Dictionary<string, object> SettingsDict { get; set; }
    public IFileService FileService { get; set; }
    public IOperationsService OperationsService { get; set; }
    private IConfigService ConfigService { get; set; }
    public override void Registrations()
    {
        // MODULE SERVICE
        OutContainer.RegisterByFunc<IFileService>(
            () => FileService);
        
        // MODULE SERVICE
        OutContainer.RegisterByFunc<IOperationsService>(
            () => OperationsService);
        
        // MODULE SERVICE
        ConfigService = OutBorder2.ConfigService(OperationsService);
        OutContainer.RegisterByFunc<IConfigService>(
            () => ConfigService);

        // BACKEND MODULE - RepoService
        IRepoService repo = OutBorder3.RepoService(FileService);
        OutContainer.RegisterByFunc<IFileService, IRepoService>(
            x => repo,
            () => FileService,
            0,
            InitGroupsFromSearchPaths);

        // MODULE SERVICE
        IRepoOperationsService repoOp = OutBorder1.RepoOperationsService(
            FileService,
            repo);
        OutContainer.RegisterByFunc<IRepoOperationsService>( 
            () => repoOp);
        
        // MODULE SERVICE
        IGoogleDriveService googleDrive = OutBorder4.GoogleDriveService(
            ConfigService.SettingsDict,
            OperationsService);
        OutContainer.RegisterByFunc<IGoogleDriveService>(
            () => googleDrive);
        
        // MODULE SERVICE
        IGoogleDocsService googleDoc = OutBorder5.GoogleDocsService(
            ConfigService.SettingsDict);
        OutContainer.RegisterByFunc<IGoogleDocsService>(
            () => googleDoc);

        // MODULE SERVICE
        IGoogleSheetService googleSheet = OutBorder6.GoogleSheetService(
            ConfigService.SettingsDict);
        OutContainer.RegisterByFunc(
            () => googleSheet);
        
        // var argsManager = OutBorder11.ArgsManagerService();
        // OutContainer.RegisterByFunc<IArgsManagerService>(
        //     () => argsManager);
        
        OutContainer.RegisterByFunc(
            (x) => OutBorder8.VideoService(x),
            () => OutContainer.Resolve<IOperationsService>());

        // MAIN BUTTON ACTIONS MODULE
        IMainButtonActionsService buttonActions = OutBorder15
            .MainButtonActionsService(OperationsService, repo);
        OutContainer.RegisterByFunc(
            () => buttonActions);
        
        // OutContainer.RegisterByFunc(
        //     (x) => OutBorder7.TtsService(),
        //     () => OutContainer.Resolve<IOperationsService>());
        
        ITtsService ttsService = OutBorder7.TtsService(
            OutContainer.Resolve<IOperationsService>(),
            OutContainer.Resolve<IRepoService>(),
            OutContainer.Resolve<IVideoService>());
        OutContainer.RegisterByFunc(() => ttsService);
        OutContainer.ServiceCollection.AddSpeechSynthesis();
        
        
        
        // BACKEND MODULE
        IIdentityService identity = OutBorder16.IdentityService();
        OutContainer.RegisterByFunc(
            () => identity);

        IImageSpliterService imageSpliter = OutBorder13.ImageSpliterService();
        OutContainer.RegisterByFunc(() => imageSpliter);
        
        // OutContainer.ServiceRegister.AddSpeechSynthesisServices();
        OutContainer.ServiceCollection.AddSpeechSynthesis();
        
        // BACKEND MODULE - StringArgsResolver Service
        List<object> servicesList =
        [
            repo,
            buttonActions,
            identity
        ];
        IStringArgsResolverService stringArgs = OutBorder14
            .StringArgsResolverService(servicesList);
        OutContainer.RegisterByFunc(
            () => stringArgs);
        
        // BACKEND MODULE - Backend Service
        IBackendService backend = OutBorder12.BackendService();
        OutContainer.RegisterByFunc(() => backend);
    }

    private void InitGroupsFromSearchPaths()
    {
        IRepoService repoService = OutContainer.Resolve<IRepoService>();
        ConfigService.Prepare(SettingsDict);
        List<string> searchPaths = ConfigService.GetRepoSearchPaths();
        repoService.InitGroupsFromSearchPaths(searchPaths);
    }
}
