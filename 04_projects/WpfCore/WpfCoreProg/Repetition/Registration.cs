using WpfNotesSystem.ViewModels;
using SharpContainerProg.AAPublic;
using SharpRepoBackendProg.Services;
using WpfCoreProg.ViewModels;

namespace WpfNotesSystem.Repetition;

internal class Registration : RegistrationBase
{
    public override void Registrations()
    {
        // OutContainer.RegisterByFunc<IBackendService>(
        //     OutBorder3.BackendService);
        //
        // OutContainer.RegisterByFunc<MainViewModel>(
        //     () => new MainViewModel());
        // OutContainer.RegisterByFunc<TextViewModel>(
        //     () => new TextViewModel(), 1);
        // OutContainer.RegisterByFunc<FolderViewModel>(
        //     () => new FolderViewModel(), 1);
    }
}