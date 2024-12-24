using WpfNotesSystem.ViewModels;
using SharpContainerProg.AAPublic;
using SharpRepoBackendProg.Services;
using WpfCoreProg.ViewModels;
using OutBorder01 = SharpRepoBackendProg.AAPublic.OutBorder;

namespace WpfNotesSystem.Repetition;

internal class Registration : RegistrationBase
{
    public override void Registrations()
    {
        OutContainer.RegisterByFunc<IBackendService>(
            OutBorder01.BackendService);
        OutContainer.RegisterByFunc<TextViewModel>(
            () => new TextViewModel(), 1);
        OutContainer.RegisterByFunc<FolderViewModel>(
            () => new FolderViewModel(), 1);
        OutContainer.RegisterByFunc<MainViewModel>(
            () => new MainViewModel());
    }
}